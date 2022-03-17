using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands.Movement.Movements
{
   #region Movement
   public class MoveFoward : Command
   {
      IMoveable _moveable;

      public MoveFoward(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.MoveFoward();
      }
   }

   public class MoveBackword : Command
   {
      IMoveable _moveable;

      public MoveBackword(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.MoveBackword();
      }
   }

   public class MoveLeft : Command
   {
      IMoveable _moveable;

      public MoveLeft(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.MoveLeft();
      }
   }

   public class MoveRight : Command
   {
      IMoveable _moveable;

      public MoveRight(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.MoveRight();
      }
   }

   public class DashStart : Command
   {
      IMoveable _moveable;

      public DashStart(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.DashStart();
      }
   }

   public class DashStop : Command
   {
      IMoveable _moveable;

      public DashStop(IMoveable moveable)
      {
         _moveable = moveable;
      }

      public override void Execute()
      {
         _moveable.DashStop();
      }
   }

   #endregion // Movement

   public class Jump : Command
   {
      IJumpable _jumpable;

      public Jump(IJumpable jumpable)
      {
         _jumpable = jumpable;
      }

      public override void Execute()
      {
         _jumpable.Jump();
      }
   }

   public class Crouch : Command
   {
      ICrouchable _crouchable;

      public Crouch(ICrouchable crouchable)
      {
         _crouchable = crouchable;
      }

      public override void Execute()
      {
         _crouchable.Crouch();
      }
   }

   #region Weapon
   public class MouseRight : Command
   {
      IWeaponable _weaponable;

      public MouseRight(IWeaponable weaponable)
      {
         _weaponable = weaponable;
      }

      public override void Execute()
      {
         _weaponable.MouseRight();
      }
   }

   public class MouseLeft : Command
   {
      IWeaponable _weaponable;

      public MouseLeft(IWeaponable weaponable)
      {
         _weaponable = weaponable;
      }

      public override void Execute()
      {
         _weaponable.MouseLeft();
      }
   }

   public class ResetKey : Command
   {
      IWeaponable _weaponable;

      public ResetKey(IWeaponable weaponable)
      {
         _weaponable = weaponable;
      }
      public override void Execute()
      {
         _weaponable.R();
      }
   }
   #endregion // Weapon
}