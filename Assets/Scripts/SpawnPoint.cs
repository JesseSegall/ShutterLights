using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
  public string spawnPointID;

  public Color gizmoColor = Color.yellow;

  public float gizmoSize = .6f;

  private void OnDrawGizmos()
  {
    Gizmos.color = gizmoColor;
    Gizmos.DrawWireSphere(transform.position, gizmoSize);

    Gizmos.DrawRay(transform.position, transform.forward * gizmoSize);
  }

}
