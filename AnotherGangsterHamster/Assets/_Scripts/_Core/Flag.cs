/// <summary>
/// 소유권 관리 위함
/// </summary>
public class Flag
{
   private           bool _status;
   private readonly  bool _autoReset;

   public Flag(bool initalStatus,
               bool autoResetEvent = true)
   {
      _status     = initalStatus;
      _autoReset  = autoResetEvent;
   }

   public void Set() => _status = true;

   public bool Get()
   {
      if (_status)
      {
         if (_autoReset) _status = false;
         return true;
      }

      return false;
   }
}