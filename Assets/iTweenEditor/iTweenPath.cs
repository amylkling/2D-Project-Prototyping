//by Bob Berkebile : Pixelplacement : http://www.pixelplacement.com

using UnityEngine;
using System.Collections.Generic;

public class iTweenPath : MonoBehaviour
{
	public string pathName ="";
	public Color pathColor = Color.cyan;
	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	public List<Vector3> globalNodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	public int nodeCount;
	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();
	public bool initialized = false;
	public string initialName = "";
	public bool sealedPath = false;
	
	void OnEnable(){
		paths.Add(pathName.ToLower(), this);
	}
	
	void OnDrawGizmosSelected(){
		if(enabled) { // dkoontz
			if(nodes.Count > 0){
				Vector3[] globalPoints = new Vector3[nodes.Count];
				for (int i = 0; i < nodes.Count; i++)
				{
					globalPoints[i] = gameObject.transform.TransformPoint(nodes[i]);
				}
				iTween.DrawPath(globalPoints, pathColor);
			}
		} // dkoontz
	}
	
	public static Vector3[] GetPath(string requestedName){
		requestedName = requestedName.ToLower();
		if(paths.ContainsKey(requestedName)){
			iTweenPath path = paths[requestedName];
			Vector3[] globalPoints = new Vector3[path.nodes.Count];
			for (int i = 0; i < path.nodes.Count; i++)
			{
				globalPoints[i] = path.gameObject.transform.TransformPoint(path.nodes[i]);
			}
			return globalPoints;
		}else{
			Debug.Log("No path with that name exists! Are you sure you wrote it correctly?");
			return null;
		}
	}
}

