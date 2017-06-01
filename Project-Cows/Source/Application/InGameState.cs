﻿/// Project: Cow Racing
/// Developed by GearShift Games, 2015-2016
///     D. Sinclair
///     N. Headley
///     D. Divers
///     C. Fleming
///     C. Tekpinar
///     D. McNally
///     G. Annandale
///     R. Ferguson
/// ================
/// InGameState.cs

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using FarseerPhysics.Dynamics;

using Project_Cows.Source.Application.Entity.Vehicle;
using Project_Cows.Source.Application.Entity;
using Project_Cows.Source.Application.Track;
using Project_Cows.Source.System.Graphics;
using Project_Cows.Source.System.Graphics.Particles;
using Project_Cows.Source.System.Graphics.Sprites;
using Project_Cows.Source.System.Input;
using Project_Cows.Source.System.StateMachine;
using Project_Cows.Source.System;

namespace Project_Cows.Source.Application {
	class InGameState : State {
		// Class to handle the in game state of the game
		// ================

		// Variables

        // <Farseer>
        World fs_world;
        // </Farseer>

        private TrackHandler h_trackHandler = new TrackHandler();
        private List<AnimatedSprite> m_animatedSprites = new List<AnimatedSprite>();
        private List<Particle> m_particles = new List<Particle>();
        private Timer startTimer = new Timer();

        private Sprite m_background;
        private Sprite m_grassbackground;

        private List<Sprite> m_rankingSprites = new List<Sprite>();
      //  private List<int> m_rankings = new List<int>();

        int m_winner = 0;

        bool finished;

        //will be used to determine if all players have readied up
        bool PlayersReady;
        int NoPlayersReady;

		SoundEffectInstance countdownBeeps;

		// Methods
		public InGameState() : base() {
			// InGameState constructor
			// ================

			m_currentState = GameState.IN_GAME;

			m_currentExecutionState = ExecutionState.INITIALISING;
		}

		public override void Initialise() {
			// Initialise in-game state
			// ================

            fs_world = new FarseerPhysics.Dynamics.World(Vector2.Zero);

            m_background = new Sprite(TextureHandler.gameTrack, new Vector2(Settings.m_screenWidth / 2, Settings.m_screenHeight / 2), 0.0f, Vector2.One);
			m_grassbackground = new Sprite(TextureHandler.gameBackground, new Vector2(0.0f, 0.0f), 0.0f, new Vector2(2.0f, 2.0f));

            h_trackHandler.Initialise(fs_world);

            //Ready Up Stuff
			PlayersReady = false;//true;
            NoPlayersReady = 0;

            // Initialise rankings
            m_rankings = new List<int>();
            m_rankings.Clear();

			// Initialise players
            m_players = new List<Player>();
            m_players.Clear();

            int playerIndex = 0;

            if (Settings.m_joinedPlayers[0]) {
				m_players.Add(new Player(fs_world, TextureHandler.player1Cow, TextureHandler.player1Vehicle, TextureHandler.readyUnselectedButton, new Vector2(100, Settings.m_screenHeight - 100), 0, h_trackHandler.m_vehicles[playerIndex], 0, Quadrent.BOTTOM_LEFT, playerIndex + 1));
                m_players[m_players.Count-1].m_controlScheme.SetSteeringSprite(new Sprite(TextureHandler.controlWheelBlue, new Vector2(100.0f, 100.0f), 0, new Vector2(1.0f, 1.0f), true));
                m_players[m_players.Count-1].m_controlScheme.SetInterfaceSprite(new Sprite(TextureHandler.controlSliderBackground, new Vector2(100.0f, 100.0f), 0, new Vector2(1.0f, 1.0f), true));
				m_rankingSprites.Add(new Sprite(TextureHandler.player1Vehicle, new Vector2(Settings.m_screenWidth / 2 - (500 - (m_players.Count * 100)), Settings.m_screenHeight / 2 - 30), 0, new Vector2(1.0f, 1.0f), true));
                playerIndex++;
            }
            if (Settings.m_joinedPlayers[1]) {
				m_players.Add(new Player(fs_world, TextureHandler.player2Cow, TextureHandler.player2Vehicle, TextureHandler.readyUnselectedButton, new Vector2(Settings.m_screenWidth - 100, Settings.m_screenHeight - 100), 0, h_trackHandler.m_vehicles[playerIndex], 0, Quadrent.BOTTOM_RIGHT, playerIndex + 1));
				m_players[m_players.Count - 1].m_controlScheme.SetSteeringSprite(new Sprite(TextureHandler.controlWheelOrange, new Vector2(100.0f, 100.0f), 0, new Vector2(1.0f, 1.0f), true));
				m_players[m_players.Count - 1].m_controlScheme.SetInterfaceSprite(new Sprite(TextureHandler.controlSliderBackground, new Vector2(100.0f, 100.0f), 0, new Vector2(1.0f, 1.0f), true));
				m_rankingSprites.Add(new Sprite(TextureHandler.player2Vehicle, new Vector2(Settings.m_screenWidth / 2 - (500 - (m_players.Count * 100)), Settings.m_screenHeight / 2 - 30), 0, new Vector2(1.0f, 1.0f), true));
                playerIndex++;
            }
            if (Settings.m_joinedPlayers[2]) {
				m_players.Add(new Player(fs_world, TextureHandler.player3Cow, TextureHandler.player3Vehicle, TextureHandler.readyUnselectedButton, new Vector2(100, 100), 180, h_trackHandler.m_vehicles[playerIndex], 0, Quadrent.TOP_LEFT, playerIndex + 1));
				m_players[m_players.Count - 1].m_controlScheme.SetSteeringSprite(new Sprite(TextureHandler.controlWheelPurple, new Vector2(100.0f, 100.0f), 180, new Vector2(1.0f, 1.0f), true));
				m_players[m_players.Count - 1].m_controlScheme.SetInterfaceSprite(new Sprite(TextureHandler.controlSliderBackground, new Vector2(100.0f, 100.0f), 180, new Vector2(1.0f, 1.0f), true));
				m_rankingSprites.Add(new Sprite(TextureHandler.player3Vehicle, new Vector2(Settings.m_screenWidth / 2 - (500 - (m_players.Count * 100)), Settings.m_screenHeight / 2 - 30), 0, new Vector2(1.0f, 1.0f), true));
                playerIndex++;
            }
            if (Settings.m_joinedPlayers[3]) {
				m_players.Add(new Player(fs_world, TextureHandler.player4Cow, TextureHandler.player4Vehicle, TextureHandler.readyUnselectedButton, new Vector2(Settings.m_screenWidth - 100, 100), 180, h_trackHandler.m_vehicles[playerIndex], 0, Quadrent.TOP_RIGHT, playerIndex + 1));
				m_players[m_players.Count - 1].m_controlScheme.SetSteeringSprite(new Sprite(TextureHandler.controlWheelYellow, new Vector2(100.0f, 100.0f), 180, new Vector2(1.0f, 1.0f), true));
				m_players[m_players.Count - 1].m_controlScheme.SetInterfaceSprite(new Sprite(TextureHandler.controlSliderBackground, new Vector2(100.0f, 100.0f), 180, new Vector2(1.0f, 1.0f), true));
				m_rankingSprites.Add(new Sprite(TextureHandler.player4Vehicle, new Vector2(Settings.m_screenWidth / 2 - (500 - (m_players.Count * 100)), Settings.m_screenHeight / 2 - 30), 0, new Vector2(1.0f, 1.0f), true));
                playerIndex++;
            }

            // Initialise sprites
           
			// Music
			//MediaPlayer.Play(AudioHandler.raceMusic);
			//MediaPlayer.IsRepeating = true;

			countdownBeeps = AudioHandler.countdownBeeps.CreateInstance();

            // Start timer
            startTimer.StartTimer(3000.0f);

            finished = false;

			// Set initial next state
			m_nextState = GameState.VICTORY_SCREEN;

			// Change execution state
			m_currentExecutionState = ExecutionState.RUNNING;
		}

        public override void Update(ref TouchHandler touchHandler_, GameTime gameTime_)
        {
            // Update in game state
            // ================

            // Update touch input handler
            touchHandler_.Update();

            // Create lists to contain touches for each player
            List<List<TouchLocation>> playerTouches = new List<List<TouchLocation>>();
            for (int p = 0; p < m_players.Count; ++p)
            {
                playerTouches.Add(new List<TouchLocation>());
            }

            // Iterate through each player and sort out which touches are for which player
            foreach (TouchLocation tl in touchHandler_.GetTouches())
            {
                for (int index = 0; index < playerTouches.Count; ++index)
                {
                    if (m_players[index].m_controlScheme.GetTouchZone().IsInsideZone(tl.Position))
                    {
                        playerTouches[index].Add(tl);
                    }
                }
            }

            //only want to do this when the main game isnt running
            if (!PlayersReady)
            {
                //check if a players ready up button has been pressed
                foreach (TouchLocation tl in touchHandler_.GetTouches())
                {
                    for (int index = 0; index < m_players.Count; ++index)
                    {
                        if (m_players[index].m_ReadyButton.Activated(tl.Position))
                        {
                            m_players[index].m_ReadyUp = true;
                            m_players[index].m_ReadyButton.m_sprite.SetTexture(TextureHandler.readySelectedButton);
                        }
                    }
                }

                //Set to 0 every run, so that it doesnt count multiple touches from same person
                NoPlayersReady = 0;

                foreach (Player p in m_players)
                {
                    if (p.m_ReadyUp == true)
                    {
                        NoPlayersReady += 1;
                    }
                }

                //When all players have readied up, start main game
                if (NoPlayersReady == m_players.Count)
                {
					countdownBeeps.IsLooped = false;
					countdownBeeps.Play();
                    PlayersReady = true;
                }
                h_trackHandler.Update(m_players, ref m_rankings);
            }
            else if (PlayersReady)
            {
                startTimer.Update(gameTime_.ElapsedGameTime.Milliseconds);
                if (startTimer.timerFinished)
                {
                    for (int i = 0; i < m_players.Count; ++i)
                    {
                        if (m_players[i].m_currentLap == Settings.m_number_laps)
                        {
                            m_players[i].SetFinished(true);
                            m_players[i].GetVehicle().SetToSensor();
                            m_players[i].AddFinishTime(gameTime_.ElapsedGameTime.Milliseconds);
                            if (m_winner == 0) {
                                m_winner = m_players[i].GetID();
                            }
                        }
                        if (!m_players[i].GetFinished() || (m_players[i].GetFinished() && m_players[i].GetFinishTime() < 500))
                        {
                            bool left = false;
                            bool right = false;
                            bool brake = false;
                            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                            {
                                left = true;
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                            {
                                right = true;
                            }
                            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Down))
                            {
                                brake = true;
                            }

                            m_players[i].KeyboardMove(left, right, brake);
                            m_players[i].Update(playerTouches[i], m_rankings[i]);
                            m_players[i].AddRaceTime(gameTime_.ElapsedGameTime.Milliseconds);
                        }
                    }
                }
                // Victory state

                finished = true;
                foreach (Player p in m_players)
                {
                    if (!p.GetFinished())
                    {
                        finished = false;
                        break;
                    }
                }
                if (finished)
                {
                    m_currentExecutionState = ExecutionState.CHANGING;
                }

                // Update game objects

                float turn = 0;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    turn--;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    turn++;
                }
                bool braked = Keyboard.GetState().IsKeyDown(Keys.Down);


                h_trackHandler.Update(m_players, ref m_rankings);

                // Update sprites
                foreach (AnimatedSprite anim in m_animatedSprites)
                {
                    // If currently animating
                    if (anim.GetCurrentState() == 1)
                    {
                        // Change the frame
                        if (gameTime_.TotalGameTime.TotalMilliseconds - anim.GetLastTime() > anim.GetFrameTime())
                        {
                            anim.ChangeFrame(gameTime_.TotalGameTime.TotalMilliseconds);
                        }
                    }
                }

                foreach (CheckpointContainer cp in h_trackHandler.m_checkpoints)
                {
                    cp.GetEntity().UpdateSprites();
                }

                //this set the sprite for the rankings, to the players to the corresponding rank
                for (int i = 0; i < m_rankings.Count; i++)
                {
                    m_rankingSprites[i].SetTexture(m_players[m_rankings[i] - 1].GetVehicle().m_vehicleBody.GetSprite().GetTexture());
                }
              
                // Update particles
                m_particles = GraphicsHandler.UpdatePFX(gameTime_.ElapsedGameTime.TotalMilliseconds);

                fs_world.Step((float)gameTime_.ElapsedGameTime.TotalMilliseconds * .001f);
            }
        }

		public override void Draw(GraphicsDevice graphicsDevice_) {
			// Render objects to the screen
			// ================
          
			// Clear the screen
			graphicsDevice_.Clear(Color.Beige);

            // Start rendering graphics
            GraphicsHandler.StartDrawing();

            // Render background
            GraphicsHandler.DrawSprite(m_grassbackground);
            GraphicsHandler.DrawSprite(m_background);

            // Render animated sprites
            foreach (AnimatedSprite anim_ in m_animatedSprites) {
                if (anim_.IsVisible()) {
                    GraphicsHandler.DrawAnimatedSprite(anim_);
                }
            }

            // Render particles             TEMP
            foreach (Particle part_ in m_particles) {
                if (part_.GetLife() > 0) {
                    GraphicsHandler.DrawParticle(part_.GetPosition(), part_.GetColour(), part_.GetLife());
                }
            }

            h_trackHandler.Draw();

            // Render player vehicles
			foreach(Player p in m_players) {
                GraphicsHandler.DrawSprite(p.GetVehicle().m_vehicleBody.GetSprite());
                //GraphicsHandler.DrawSprite(p.GetCow());
                GraphicsHandler.DrawSprite(p.m_controlScheme.m_controlInterfaceSprite);
                GraphicsHandler.DrawSprite(p.m_controlScheme.m_steeringIndicatorSprite);

                if(!PlayersReady)
                {
                    GraphicsHandler.DrawSprite(p.m_ReadyButton.m_sprite);
                }
			}

            for (int i = 0; i < Settings.m_numberOfPlayers; i ++)
            {
                //GraphicsHandler.DrawSprite(m_rankingSprites[i]);
            }

                // RENDER UI

                // Render ranking text
                if (m_rankings.Count != 0)
                {
                    GraphicsHandler.DrawText("1st - Player " + m_rankings[0].ToString(), new Vector2(Settings.m_screenWidth / 2 - 450, Settings.m_screenHeight / 2 - 80), Color.Red, 0);
                   
                    if (m_rankings.Count > 1)
                    {
                        GraphicsHandler.DrawText("2nd - Player " + m_rankings[1].ToString(), new Vector2(Settings.m_screenWidth / 2 - 350, Settings.m_screenHeight / 2 - 80), Color.Red, 0);
                        if (m_rankings.Count > 2)
                        {
                            GraphicsHandler.DrawText("3rd - Player " + m_rankings[2].ToString(), new Vector2(Settings.m_screenWidth / 2 - 250, Settings.m_screenHeight / 2 - 80), Color.Red, 0);
                            if (m_rankings.Count > 3)
                            {
                                GraphicsHandler.DrawText("4th - Player " + m_rankings[3].ToString(), new Vector2(Settings.m_screenWidth / 2 - 150, Settings.m_screenHeight / 2 - 80), Color.Red, 0);
                            }
                        }
                    }
                }

                for (int i = 0; i < m_rankingSprites.Count; i++) {
                    GraphicsHandler.DrawSprite(m_rankingSprites[i]);
                }




                    /*if (m_rankings.Count != 0) {
                        GraphicsHandler.DrawText(new DebugText("1st - Player " + m_players[0].GetRaceTime(), new Vector2(1000f, 440f)));
                        if (m_rankings.Count > 1) {
                            GraphicsHandler.DrawText(new DebugText("2nd - Player " + m_players[1].GetRaceTime(), new Vector2(1000f, 460f)));
                            if (m_rankings.Count > 2) {
                                GraphicsHandler.DrawText(new DebugText("3rd - Player " + m_players[2].GetRaceTime(), new Vector2(1000f, 480f)));
                                if (m_rankings.Count > 3) {
                                    GraphicsHandler.DrawText(new DebugText("4th - Player " + m_players[3].GetRaceTime(), new Vector2(1000f, 500f)));
                                }
                            }
                        }
                    }*/

            foreach(Player p in m_players) {
                Debug.AddSprite(p.GetVehicle().m_vehicleBody.GetSprite());
                foreach (Tyre t in p.GetVehicle().m_vehicleTyres) {
                    Debug.AddSprite(t.GetSprite());
                }
            }

			foreach(Player p in m_players) {
				Debug.AddSprite(p.GetVehicle().m_vehicleTyres[0].GetSprite());
				Debug.AddSprite(p.GetVehicle().m_vehicleTyres[1].GetSprite());
				Debug.AddSprite(p.GetVehicle().m_vehicleTyres[2].GetSprite());
				Debug.AddSprite(p.GetVehicle().m_vehicleTyres[3].GetSprite());
			}
            //GraphicsHandler.DrawSprite(bsv.m_vehicleBody.GetSprite());*/



            if (!startTimer.timerFinished) {
                GraphicsHandler.DrawText(((int)(startTimer.timeRemaining / 1000) + 1).ToString(), new Vector2(1000, 50), Color.Red, 3);
            }
            if(finished){
                GraphicsHandler.DrawText("Player " + m_winner.ToString() + " is the winner", new Vector2(500, 500), Color.Red);
            }

            // Stop rendering graphics
            GraphicsHandler.StopDrawing();
		}

		protected override void CleanUp() {
			// Clean up any objects
			// ================

			throw new NotImplementedException();
		}

		// Getters
		public override GameState GetState() { return m_currentState; }

		public override GameState GetNextState() { return m_nextState; }

		public override ExecutionState GetExecutionState() { return m_currentExecutionState; }

		// Setters

	}
}
