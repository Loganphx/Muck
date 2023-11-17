﻿using System;
using UnityEditor.VersionControl;
using UnityEngine;

namespace ECS.Movement.Services
{
    [Serializable]
    public class PlayerInputStateComponent
    {
        public static readonly int HashId = typeof(PlayerInputStateComponent).GetHashCode();

      
        public float         mouseSensitivity       = 0.5f;
        public float         cameraVerticalRotation = 1;
        public GameplayInput Input;
    }
      /// <summary>
  /// Example definition of an INetworkStruct.
  /// </summary>
  [Serializable]
  public struct GameplayInput /* : ISerializable, IDeserializable<GameplayInput>*/
  {
    public Vector2 moveDirection;
    
    public Vector2 lookDelta;
    public float yEulerAngle;
    public float xEulerAngle;
    
    public NetworkButtons Actions;
    
    public byte weaponId;

    public bool Jump
    {
      get => Actions.IsSet(GameplayInputAction.Jump);
      set => Actions.Set(GameplayInputAction.Jump, value);
    }

    public bool Crouch
    {
      get => Actions.IsSet(GameplayInputAction.Crouch);
      set => Actions.Set(GameplayInputAction.Crouch, value);
    }
    
    public bool Sprint
    {
      get => Actions.IsSet(GameplayInputAction.Sprint);
      set => Actions.Set(GameplayInputAction.Sprint, value);
    }

    public bool Interact
    {
      get => Actions.IsSet(GameplayInputAction.Interact);
      set => Actions.Set(GameplayInputAction.Interact, value);
    }
    public bool Reload
    {
      get => Actions.IsSet(GameplayInputAction.Reload);
      set => Actions.Set(GameplayInputAction.Reload, value);
    }

    public bool PrimaryAttackClicked
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Primary);
      set => Actions.Set(GameplayInputAction.Attack_Primary, value);
    }
    
    public bool PrimaryAttackHeld
    {
      get => Actions.IsSet(GameplayInputAction.Attack_Primary);
      set => Actions.Set(GameplayInputAction.Attack_Primary, value);
    }

    public bool Aim
    {
      get => Actions.IsSet(GameplayInputAction.Aim);
      set => Actions.Set(GameplayInputAction.Aim, value);
    }

    public bool Block
    {
      get => Actions.IsSet(GameplayInputAction.Block);
      set => Actions.Set(GameplayInputAction.Block, value);
    }

    public bool FlashLight
    {
      get => Actions.IsSet(GameplayInputAction.FlashLight);
      set => Actions.Set(GameplayInputAction.FlashLight, value);
    }
  
    public bool Kick
    {
      get => Actions.IsSet(GameplayInputAction.Kick);
      set => Actions.Set(GameplayInputAction.Kick, value);
    }

    public bool Heal
    {
      get => Actions.IsSet(GameplayInputAction.Heal);
      set => Actions.Set(GameplayInputAction.Heal, value);
    }

    public bool Action1
    {
      get => Actions.IsSet(GameplayInputAction.Action1);
      set => Actions.Set(GameplayInputAction.Action1, value);
    }

    public bool Action2
    {
      get => Actions.IsSet(GameplayInputAction.Action2);
      set => Actions.Set(GameplayInputAction.Action2, value);
    }

    public bool Action3
    {
      get => Actions.IsSet(GameplayInputAction.Action3);
      set => Actions.Set(GameplayInputAction.Action3, value);
    }

    public bool Action4
    {
      get => Actions.IsSet(GameplayInputAction.Action4);
      set => Actions.Set(GameplayInputAction.Action4, value);
    }

    public bool Action5
    {
      get => Actions.IsSet(GameplayInputAction.Action5);
      set => Actions.Set(GameplayInputAction.Action5, value);
    }
    
    public bool ToggleCursor
    {
      get => Actions.IsSet(GameplayInputAction.ToggleCursor);
      set => Actions.Set(GameplayInputAction.ToggleCursor, value);
    }
    
    public bool Escape
    {
      get => Actions.IsSet(GameplayInputAction.Escape);
      set => Actions.Set(GameplayInputAction.Escape, value);
    }

    // public void Serialize(Message message)
    // {
    //   message.AddVector3(moveDirection);
    //   message.AddFloat(yEulerAngle);
    //   message.AddFloat(xEulerAngle);
    //   message.AddInt(Actions.Bits);
    //   message.AddByte(weaponId);
    // }

    public int Size => 25;

    // public GameplayInput Deserialize(Message message)
    // {
    //   moveDirection = message.GetVector3();
    //   yEulerAngle   = message.GetFloat();
    //   xEulerAngle   = message.GetFloat();
    //   Actions       = (NetworkButtons) message.GetInt();
    //   weaponId      = message.GetByte();
    //
    //   return this;
    // }
  }
}