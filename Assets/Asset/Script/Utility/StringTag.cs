using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Simple component to mark objects with a given guid.
/// </summary>
public class StringTag : MonoBehaviour
{

    /// <summary>
    /// Type
    /// </summary>
    public string type;


    /// <summary>
    /// Tags
    /// </summary>
    public List<string> tagList = new List<string>();

    /// <summary>
    /// Gets the first value.
    /// </summary>
	public string value { get { return tagList.Count <= 0 ? "" : tagList[0]; } }

    /// <summary>
    /// Returns a flag indicating if the informed tag is contained in the set.
    /// </summary>
    /// <param name="p_tag"></param>
    /// <returns></returns>
	public bool Contains(string p_tag) { return tagList.IndexOf(p_tag)>=0; }

    /// <summary>
    /// Returns a flag indicating if the informed tags match this component's set.
    /// </summary>
    /// <param name="p_tags"></param>
    /// <returns></returns>
    public bool Match(params string[] p_tags) { return Match(false, p_tags); }
    
    /// <summary>
    /// Returns a flag indicating if the informed tags match this component's set.
    /// If 'precise' all tags must match in number and comparison.
    /// </summary>
    /// <param name="p_precise"></param>
    /// <param name="p_tags"></param>
    /// <returns></returns>
    public bool Match(bool p_precise,params string[] p_tags)
    {
		if (p_precise) if (p_tags.Length != tagList.Count) return false;
        for(int i=0;i<p_tags.Length;i++)
        {
            bool found = false;
			for (int j = 0; j < tagList.Count; j++) if (tagList[j] == p_tags[i]) found = true;
            if (!found) return false;
        }
        return true;
    }
}

