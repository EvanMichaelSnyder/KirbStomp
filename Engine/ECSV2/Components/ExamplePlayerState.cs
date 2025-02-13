using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.Components.IComponents;
using Microsoft.Xna.Framework;


namespace KirbStomp.Engine.ECSV2.Components
{
	internal class ExamplePlayerState : Component
	{
		private string name;
		private string spriteSheetName;
		private int playerMaxHealth;
		private int playerHealth;
		
		private float playerJumpVelocity;
		private bool playerJumped;
		private bool playerDoubleJumped;

		private Vector2 walkingDirection;
		private float movementVelocity;

		private int maxArialJumpCount;
		private int jumpsRemaining;
		private bool isInAir;


		private bool startedWalkingRight;
		private bool startedWalkingLeft;
		private bool startedWalking;
		private bool stoppedWalkingRight;
		private bool stoppedWalkingLeft;
		private bool stoppedWalking;

		private bool tryingToWalkLeft;
		private bool tryingToWalkRight;
		private bool walkingLeft;
		private bool walkingRight;
		private bool isWalking;
		public ExamplePlayerState(string name)
		{
			this.name = name;
			this.playerMaxHealth = 100;
			this.playerHealth = 100;
			this.playerJumpVelocity = 250;
			this.movementVelocity = 200;
			this.maxArialJumpCount = 2;
			this.jumpsRemaining = 0;
			this.isInAir = false;

			this.startedWalkingRight = false;
			this.startedWalkingLeft = false;			
			this.startedWalking = false;
			this.stoppedWalkingRight = false;
			this.stoppedWalkingLeft = false;
			this.stoppedWalking = false;
			this.tryingToWalkLeft = false;
			this.tryingToWalkRight = false;
			this.walkingLeft = false;
			this.walkingRight = false;
		}
		public string GetWalkingAnimation()
		{
			if (walkingDirection.X < 0 ) return "RunLeft";
			if (walkingDirection.X > 0) return "Run";
			return "Idle";
		}

		public string GetPlayerName()
		{
			return this.name;
		}
		public void SetPlayerName(string name)
		{
			this.name = name;
		}
		public int GetPlayerHealth()
		{
			return this.playerHealth;
		}
		public void SetPlayerHealth(int newHealth)
		{
			this.playerHealth = newHealth;
			if(playerHealth < 0)
			{
				playerHealth = 0;
			}
		}
		public int DamagePlayer(int damageAmount)
		{
			int output = 0;
			this.playerHealth -= damageAmount;
			if(playerHealth < 0)
			{
				output = -1 * this.playerHealth;
				this.playerHealth = 0;
			}
			return output;	
		}
		public int GetPlayerMaxHealth()
		{
			return this.playerMaxHealth;
		}
		public void SetPlayerMaxHealth(int maxHealth)
		{
			this.playerMaxHealth = maxHealth;
		}
		public void SetWalkingDirection(Vector2 walkingDirection)
		{
			this.walkingDirection = walkingDirection;
			if(this.walkingDirection.Length() != 0)
				this.walkingDirection.Normalize();

		}
		public Vector2 GetWalkingDirection()
		{
			return new Vector2(this.walkingDirection.X, this.walkingDirection.Y);
		}
		public float GetMovementVelocity()
		{
			return this.movementVelocity;
		}
		public void SetMovemetnVelocity(float vel)
		{
			this.movementVelocity = vel;
		}
		public void AddMovementVelocity(float vel)
		{
			this.movementVelocity += vel;
		}

		public float GetJumpVelocity()
		{
			return this.playerJumpVelocity;
		}
		public void SetJumpVelocity(float newJumpVel)
		{
			this.playerJumpVelocity = newJumpVel;
		}

		public int SetRemainingArialJumps(int jumpsLeft)
		{
			this.jumpsRemaining = jumpsLeft;
			return jumpsLeft > maxArialJumpCount ? jumpsLeft - maxArialJumpCount : 0;
		}
		public int GetRemainingArialJumps()
		{
			return this.jumpsRemaining;
		}
		public void UpdateJumpState(bool jumped, bool isOnGround)
		{
			if (isOnGround) this.jumpsRemaining = maxArialJumpCount;
			if(jumped)
			{
				
			}
		}
		
		public void TryToMoveLeft(bool movingLeft)
		{
			this.tryingToWalkLeft = movingLeft;
		}
		public void TryToMoveRight(bool movingRight)
		{
			this.tryingToWalkRight = movingRight;
		}

		public bool GetStartedWalkingState()
		{
			return this.startedWalking;
		}
		public bool GetStoppedWalkingState()
		{
			return this.stoppedWalking;
		}

		public void UpdateMovementState()
		{
			bool wasWalking = this.isWalking;
			this.isWalking = this.tryingToWalkLeft ^ this.tryingToWalkRight;
			if (this.stoppedWalking)
			{
				this.stoppedWalking = false;
			}
			if (this.startedWalking) this.startedWalking = false;
			SetWalkingDirection(new Vector2(Convert.ToInt32(this.tryingToWalkRight) - Convert.ToInt32(this.tryingToWalkLeft), 0));
			if(wasWalking ^ isWalking)
			{
				this.startedWalking = isWalking;
				this.stoppedWalking = !isWalking;
			}

		}

		public void SetStoppedWalkingState(bool state)
		{
			this.stoppedWalking = state;
		}
	}
}