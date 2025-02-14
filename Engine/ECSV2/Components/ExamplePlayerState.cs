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
		private bool playerDoubleJumped;

		private Vector2 walkingDirection;
		private float movementVelocity;

		private int maxArialJumpCount;
		private int jumpsRemaining;
		private bool tryToJump;
		private bool isInAir;
		private bool playerJumped;


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

		private List<string> attacks;
		private Queue<string> attackQueue;
		private bool tryingToAttack;
		private bool justAttacked;
		private int attackNum;
		public ExamplePlayerState(string name)
		{
			this.name = name;
			this.playerMaxHealth = 100;
			this.playerHealth = 100;
			this.playerJumpVelocity = -480;
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

			this.attacks = new()
			{
				"AttackNeutral1", "AttackNeutral2", "AttackNeutral3"
			};
			this.tryingToAttack = false;
			this.justAttacked = false;
			this.attackQueue = new();
			
		}
		public string GetWalkingAnimation()
		{
			if (walkingDirection.X < 0 ) return "RunLeft";
			if (walkingDirection.X > 0) return "Run";
			return "Idle";
		}
		public string GetAttackAnimation()
		{
			string output = "Idle";
			if (attackQueue.Count > 0)
			{
				output = attackQueue.Dequeue();
				attackQueue.Clear();
			}
			return output;
		}
		private string GetAttackFromNum(int num)
		{
			if(num >= attacks.Count())
			{
				num = num % attacks.Count();
			}
			return attacks[num];
		}
		public bool PlayerAttacks()
		{
			return this.justAttacked;
		}
		public int TryDamagePlayer(int damageAmount, bool damageSelf)
		{
			if (damageSelf)
			{
				return DamagePlayer(damageAmount);
			}
			return 0;
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

			Logger.Log($"PlayerDamaged Healt {this.playerHealth}");
			return output;	
		}

		public void TryToAttack(int attackNum, bool attacks)
		{
			if(attacks)
			{
				this.attackNum = attackNum;
				attackQueue.Enqueue(GetAttackFromNum(attackNum));
				this.tryingToAttack = true;
			}
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
		public void TryToJump(bool jump)
		{
			this.tryToJump = jump;
		}
		public bool GetPlayerJumped()
		{
			return this.playerJumped;
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
			// Hard coded movement stats, to be changed
			bool wasWalking = this.isWalking;
			this.isWalking = this.tryingToWalkLeft ^ this.tryingToWalkRight;
			// Reset variables that are true for an instnace
			this.stoppedWalking = false;
			this.playerJumped = false;
			this.startedWalking = false;
			this.justAttacked = false;

			
			SetWalkingDirection(new Vector2(Convert.ToInt32(this.tryingToWalkRight) - Convert.ToInt32(this.tryingToWalkLeft), 0));
			if(wasWalking ^ isWalking)
			{
				this.startedWalking = isWalking;
				this.stoppedWalking = !isWalking;
			}
			// Hard coded jump stats
			if(this.tryToJump && (!this.isInAir || this.jumpsRemaining > 0))
			{
				playerJumped = true;
			}
			// Attacking
			if(this.tryingToAttack)
			{
				this.justAttacked = true;
				this.tryingToAttack = false;
			}
		}

		public void SetStoppedWalkingState(bool state)
		{
			this.stoppedWalking = state;
		}
	}
}