using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using Utility;
using Player;

public class InputManager :  DragHandler
{
    public Unit moveUnit;

    private List<Tile> recordTile = new List<Tile>();
    private GameManager gameManager;
    private GameUIManager gameUI;
    public Map _Map;

    //Current during dragging event
    private GridHolder currentHolder;

    User player { get { return transform.Find("player").GetComponent<User>(); } }

    // Use this for initialization
    public void SetUp(GameManager p_gamemanager, GameUIManager p_gameUI)
    {
        inputState = States.Idle;
        gameManager = p_gamemanager;
        gameUI = p_gameUI;
        _Map = gameManager.map;
    }

    //========================================================= General Input Command =========================================================
    public void FreePanel()
    {
        inputState = InputManager.States.Idle;
        _Map.gridManager.ResetGrid(_Map.grids);
        _Map.gridManager.ClearPathLine();

        gameUI.actionMenu.SetBool("isOpen", false);
    }

    /// <summary>
    /// Kepp tracking the best path to the point position
    /// </summary>
    public void PathTracking(Unit p_unit)
    {
        int T_layer = GeneralSetting.terrainLayer;

        GridHolder gridHolder = GetGridCollider( Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) );
        if (gridHolder == null) return;

        List<Tile> tiles = FindPath(p_unit.unitPos, gridHolder.transform.position);
        recordTile = tiles;

        List<Vector2> pathList = tiles.Select(x => x.position).ToList();
        pathList.Insert(0, p_unit.unitPos);
        _Map.gridManager.DrawPathLine(pathList);
    }

    //Get grid gollider from raycasting and its movable
    public GridHolder GetGridCollider(Vector3 p_position) {
        int T_layer = GeneralSetting.terrainLayer;

        Collider2D collide = Physics2D.OverlapPoint(new Vector2(p_position.x, p_position.y), T_layer);

        if (collide == null || collide.tag != "Ground" || collide.GetComponent<GridHolder>().gridStatus != GridHolder.Status.Move) return null;

        return collide.GetComponent<GridHolder>();
    }

    /// <summary>
    /// Call A* function
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="startPos">Start position.</param>
    /// <param name="endPos">End position.</param>
    public List<Tile> FindPath(Vector2 startPos, Vector2 endPos)
    {
        GridHolder startGrid = _Map.FindTileByPos(startPos);
        GridHolder endGrid = _Map.FindTileByPos(endPos);
        return _Map.gridManager.aPathFinding.FindPath(startGrid, endGrid);
    }

    /// <summary>
    /// Moves unit with given paths.
    /// </summary>
    /// <param name="p_moveunit">P moveunit.</param>
    /// <param name="tiles">Tiles.</param>
    /// <param name="callback">Callback.</param>
    public void MoveUnitFromPath(Unit p_moveunit, List<Tile> tiles, System.Action callback = null)
    {
        if (tiles.Count <= 0)
        {
            _Map.gridManager.DrawPathLine(new List<Vector2>());
            if (callback != null) callback();
            return;
        }

        Vector3[] path = tiles.ConvertAll<Vector3>(x => x.position).ToArray();

        p_moveunit.Move(path, delegate ()
        {
            //Clear Path Line
            _Map.gridManager.ClearPathLine();
            if (callback != null) callback();
        });
    }

    public void MoveUnitFromDrop(Unit p_moveunit, Tile p_lastTiles)
    {
        p_moveunit.unitPos = p_lastTiles.position;
        p_moveunit.status = Unit.Status.Moved;


        
        
        gameManager.gameState.VerifyTile(_Map.FindTileByPos(p_moveunit.unitPos), gameManager.mMapPrefab  );

        FreePanel();
    }

    //Resume unit to it
    public void ResumeUnit(Unit p_unit, bool isIdle = true)
    {
        p_unit.transform.position = p_unit.unitPos;
        p_unit.status = (isIdle) ? Unit.Status.Idle : Unit.Status.Moved;
        FreePanel();
        moveUnit = null;
    }

    public void DisplayRoute(Unit p_unit)
    {
        _Map.gridManager.FindPossibleRoute(p_unit, gameManager.enemy);
    }

    // ========================================================= Player Input Command =========================================================
    void Update()
    {
        if (gameManager == null) return;

        //Only work in player's turn
        if (gameManager.currentUser.userType == EventFlag.UserType.Player)
        {
            //Call Draging function
            base.OnUpdate();
        }


		//Mouse CLick


    }

    //Get all Terrain / Unit from this method, except the current unit you are selecting
    public List<Collider2D> GetAllColliderByMousePos(Vector2 p_mousePoint, Unit p_holdUnit = null)
    {
        int UT_layer = GeneralSetting.unitLayer + GeneralSetting.terrainLayer;
        //Get most top sorting collider
        List<Collider2D> collides = Physics2D.OverlapPointAll(p_mousePoint, UT_layer).ToList();
        collides.Sort((x, y) => x.GetComponent<SpriteRenderer>().sortingOrder.CompareTo(y.GetComponent<SpriteRenderer>().sortingOrder));
        collides.Reverse();

        if (p_holdUnit != null)
        {
            collides.RemoveAll(x=>x.name == p_holdUnit.name);
        }

        if (collides == null || collides.Count <= 0) return null;

        return collides;
    }


    public void OnCharacterClick( Unit p_unit ) {

    }

    //=========================================== Drag and Drop ======================================
    public override void OnDragBegin(GameObject p_gameobject) {
        if (p_gameobject == null) return;
        Unit selectedUnit = p_gameobject.GetComponent<Unit>();

        //Check if the unit is available (Player and idle)
        if (p_gameobject.tag == "Player" && p_gameobject.GetComponent<Unit>().status == Unit.Status.Idle)
        {
            base.OnDragBegin(p_gameobject);

            moveUnit = selectedUnit;
		    DisplayRoute(moveUnit);

        }

        //Show information about the selected object
        if (gameUI.topInfoView.currentUnit != selectedUnit) {
          gameUI.topInfoView.SetCharacterInfo( selectedUnit );
        }        
    }

    public override void OnDrag(Vector3 p_mousePosition) {
		if (moveUnit == null) return; 
        base.OnDrag(p_mousePosition);

        //Move Friendly Unit
		moveUnit.transform.position = new Vector2( p_mousePosition.x, p_mousePosition.y);
        GridHolder gridHolder = GetGridCollider(moveUnit.transform.position);

        if (currentHolder != gridHolder) {
    		PathTracking( moveUnit );
        }

        currentHolder = gridHolder;
    }

    public override void OnDrop(Vector3 p_mousePosition) {
        List<Collider2D> collides = GetAllColliderByMousePos(p_mousePosition, moveUnit);

        if (collides == null || collides.Count <= 0) {
            ResumeUnit(moveUnit);
            return;
        }
        Collider2D mCollide = collides[0];

        if (mCollide.tag == "Ground" && mCollide.GetComponent<GridHolder>().gridStatus == GridHolder.Status.Move)
        {

            if ( recordTile.Count == 0) {
                ResumeUnit(moveUnit);
                return;
            }

            //Normal Grid
            MoveUnitFromDrop(moveUnit, recordTile[recordTile.Count - 1]);
        }
        else if (mCollide.tag == "Enemy" && collides[1].GetComponent<GridHolder>().gridStatus == GridHolder.Status.Attack)
        {
            Debug.Log("Is Enemy");

            Unit target = mCollide.GetComponent<Unit>();
            GridHolder gridHolder = _Map.FindTileByPos(target.unitPos);
            Debug.Log( gridHolder.attackPosList.Count );
            
            Vector2 bestStandPoint = _Map.gridManager.FindBestAttackPos(gridHolder.attackPos, gridHolder.attackPosList, recordTile );
            // //Pick the nearest attackpoint, unit stand before
            // for (int i = gridHolder.attackPosList.Count - 1; i >= 0; i--) {
            //       if (recordTile.Count(x=>x.position == gridHolder.attackPosList[i]) > 0 ) {
            //             bestStandPoint = recordTile.Find(x=>x.position == gridHolder.attackPosList[i]).position;
            //             break;
            //     }
            // }

            List<Tile> tiles = FindPath(moveUnit.unitPos, bestStandPoint);
            
            moveUnit.Attack(target, gridHolder);

            //if unit need to move to hit target
            if (tiles.Count > 0) {
                MoveUnitFromDrop(moveUnit, tiles[tiles.Count - 1]);
            } else {
                //Don't move
                ResumeUnit(moveUnit, false);
            }
        }
        else
        {
            ResumeUnit(moveUnit);
        }

        gameManager.gameState.VerifyGameState();
        base.OnDrop(p_mousePosition);
	}
}