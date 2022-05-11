using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
   private GameObject _player;
   public GameObject Player
   {
      get
      {
         if (_player == null)
         {
            _player = GameObject.FindGameObjectWithTag("PLAYER_BASE");
         }
         return _player;
      }
   }

   private Rigidbody _playerRigid;
   public Rigidbody PlayerRigid
   {
      get
      {
         if (_playerRigid == null)
         {
            _playerRigid = Player.GetComponent<Rigidbody>();
         }
         return _playerRigid;
      }
   }


   /// <summary>
   /// 플레이어에게서 제일 가까운 포지션을 찾습니다.
   /// </summary>
   /// <param name="positions">목록들</param>
   /// <returns>제일 가까운 Transform</returns>
   public Transform FindClosestPosition(Transform[] positions)
   {
      float distance = float.MaxValue;
      int index = -1;

      for (int i = 0; i < positions.Length; ++i)
      {
         float distanceWithPlayer =
               Vector3.Distance(Player.transform.position,
                                positions[i].position);
                                
         if (distance >= distanceWithPlayer)
         {
            index = i;
            distance = distanceWithPlayer;
         }
      }

      return positions[index];
   }
}