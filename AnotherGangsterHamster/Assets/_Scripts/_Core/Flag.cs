/// <summary>
/// 소유권 관리 위함
/// </summary>
public class Flag
{
   protected           bool _status;
   protected readonly  bool _autoReset;

   public Flag(bool initalStatus,
               bool autoResetEvent = true)
   {
      _status     = initalStatus;
      _autoReset  = autoResetEvent;
   }

   public void Set(bool status = true) => _status = status;

   public bool Get()
   {
      if (_status)
      {
         if (_autoReset) _status = false;
         return true;
      }

      return false;
   }
   
   public bool Status() => _status;
}