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
/// MenuState.cs

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using Project_Cows.Source.System;
using Project_Cows.Source.System.Graphics;
using Project_Cows.Source.System.Graphics.Particles;
using Project_Cows.Source.System.Graphics.Sprites;
using Project_Cows.Source.System.Input;
using Project_Cows.Source.System.StateMachine;

namespace Project_Cows.Source.Application {
	class MenuState : State {
		// Class to handle the menu state of the game
		// ================

		// Variables
		private MenuScreenState m_currentScreen;
        private TouchState m_touchState;
        private Vector2 m_lastPosition;

        // Sprites
        private Sprite m_background;
        private Sprite m_teamLogo;
        private Sprite m_gameLogo;
        private Sprite m_player_1_cow;
        private Sprite m_player_1_vehicle;
        private Sprite m_player_2_cow;
        private Sprite m_player_2_vehicle;
        private Sprite m_player_3_cow;
        private Sprite m_player_3_vehicle;
        private Sprite m_player_4_cow;
        private Sprite m_player_4_vehicle;
        private Sprite m_control_scheme;
        
        // Buttons
            // Main menu
        private Button m_playButton;
        private Button m_exitButton;
        private Button m_MenuButton;
        private Button m_creditsButton;
        private Button m_controlsButton;
        private Button m_backButton;
            // Player select
        private List<PlayerSelectStruct> players = new List<PlayerSelectStruct>();
        private Button m_playerBackButton;
        private Button m_playerGoButton;

		


        private List<AnimatedSprite> m_animatedSprites = new List<AnimatedSprite>();
        private List<Particle> m_particles = new List<Particle>();

        private int counter = 0; //for selecting the correct coloured texture

		// Methods
        public MenuState() : base() {
			// MenuState constructor
			// ================

			m_currentState = GameState.MAIN_MENU;

			m_currentExecutionState = ExecutionState.INITIALISING;

            // Initialise number of players to 0
            // So number must be chosen before continuing
            Settings.m_numberOfPlayers = 0;
		}

        public override void Initialise() {
			// Initialise menu state
			// ================
            


            //playerStates
            players.Clear();
            players.Add(new PlayerSelectStruct());
            players.Add(new PlayerSelectStruct());
            players.Add(new PlayerSelectStruct());
            players.Add(new PlayerSelectStruct());

            players[0].ID = 0;
            players[1].ID = 1;
            players[2].ID = 2;
            players[3].ID = 3;

            // Initialise sprites
            m_background =      new Sprite(TextureHandler.menuMainBackground, new Vector2(Settings.m_screenWidth / 2, Settings.m_screenHeight / 2), 0, Vector2.One);
            m_teamLogo =        new Sprite(TextureHandler.teamLogo,       new Vector2(Settings.m_screenWidth - 150, Settings.m_screenHeight / 11), 0, new  Vector2(0.5f,0.5f));
            m_control_scheme =  new Sprite(TextureHandler.controlsInformation,    new Vector2(Settings.m_screenWidth * 0.5f, Settings.m_screenHeight * 0.5f), 0, Vector2.One);
            //m_gameLogo = new Sprite(TextureHandler.m_gameLogo, new Vector2(Settings.m_screenWidth / 2, Settings.m_screenHeight / 2), 0, Vector2.One);
            // Not sure about this part. Maybe Move to update function
            m_player_1_cow =    new Sprite(TextureHandler.cow1,           new Vector2(Settings.m_screenWidth * 0.50f - 10.0f, Settings.m_screenHeight * 0.75f ), 0, new Vector2(0.1f, 0.1f));
            m_player_1_vehicle =new Sprite(TextureHandler.player1Vehicle,    new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.75f), 0, Vector2.One);
            m_player_2_cow =    new Sprite(TextureHandler.cow2,           new Vector2(Settings.m_screenWidth * 0.50f - 10.0f, Settings.m_screenHeight * 0.75f), 0, new Vector2(0.1f, 0.1f));
			m_player_2_vehicle = new Sprite(TextureHandler.player2Vehicle, new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.75f), 0, Vector2.One);
			m_player_3_cow = new Sprite(TextureHandler.cow3, new Vector2(Settings.m_screenWidth * 0.50f - 10.0f, Settings.m_screenHeight * 0.75f), 0, new Vector2(0.1f, 0.1f));
			m_player_3_vehicle = new Sprite(TextureHandler.player3Vehicle, new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.75f), 0, Vector2.One);
			m_player_4_cow = new Sprite(TextureHandler.cow4, new Vector2(Settings.m_screenWidth * 0.50f - 10.0f, Settings.m_screenHeight * 0.75f), 0, new Vector2(0.1f, 0.1f));
			m_player_4_vehicle = new Sprite(TextureHandler.player4Vehicle, new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.75f), 0, Vector2.One);
            // Initialise buttons
            m_playButton =      new Button(TextureHandler.menuPlayButton,       new Vector2(Settings.m_screenWidth * 0.25f, Settings.m_screenHeight * 0.75f));
            m_controlsButton =  new Button(TextureHandler.menuControlsButton,   new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.75f));
            m_MenuButton =      new Button(TextureHandler.menuMainMenuButton,       new Vector2(Settings.m_screenWidth * 0.75f, Settings.m_screenHeight * 0.75f));
            m_creditsButton =   new Button(TextureHandler.menuCreditsButton,    new Vector2(Settings.m_screenWidth * 0.90f, Settings.m_screenHeight * 0.90f));
            m_exitButton =      new Button(TextureHandler.menuExitButton,       new Vector2(Settings.m_screenWidth * 0.75f, Settings.m_screenHeight * 0.75f));
			m_backButton = new Button(TextureHandler.backButton, new Vector2(Settings.m_screenWidth * 0.75f, Settings.m_screenHeight * 0.75f));
			//m_1Button =         new Button(TextureHandler.m_menu1,          new Vector2(Settings.m_screenWidth * 0.33f, Settings.m_screenHeight * 0.50f));
            //m_2Button =         new Button(TextureHandler.m_menu2,          new Vector2(Settings.m_screenWidth * 0.66f, Settings.m_screenHeight * 0.50f));
            //m_3Button =         new Button(TextureHandler.m_menu3,          new Vector2(Settings.m_screenWidth * 0.33f, Settings.m_screenHeight * 0.625f));
            //m_4Button =         new Button(TextureHandler.m_menu4,          new Vector2(Settings.m_screenWidth * 0.66f, Settings.m_screenHeight * 0.625f));

            // Player Select
            players[0].m_actionButton = new Button(TextureHandler.joinButton, new Vector2(Settings.m_screenWidth * 0.1f, Settings.m_screenHeight * 0.9f));
            players[1].m_actionButton = new Button(TextureHandler.joinButton, new Vector2(Settings.m_screenWidth * 0.9f, Settings.m_screenHeight * 0.9f));
            players[2].m_actionButton = new Button(TextureHandler.joinButton, new Vector2(Settings.m_screenWidth * 0.1f, Settings.m_screenHeight * 0.1f));
            players[3].m_actionButton = new Button(TextureHandler.joinButton, new Vector2(Settings.m_screenWidth * 0.9f, Settings.m_screenHeight * 0.1f));
                // Player 1 -- BOTTOM_LEFT
            players[0].m_choice1Button = new Button(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.1f, Settings.m_screenHeight * 0.7f));
            players[0].m_choice2Button = new Button(TextureHandler.vehicleLargeTractorWhite, new Vector2(Settings.m_screenWidth * 0.2f, Settings.m_screenHeight * 0.7f));
            players[0].m_choice3Button = new Button(TextureHandler.vehicleLargeTankWhite, new Vector2(Settings.m_screenWidth * 0.3f, Settings.m_screenHeight * 0.7f));
            players[0].m_choice4Button = new Button(TextureHandler.vehicleLargeBuggyWhite, new Vector2(Settings.m_screenWidth * 0.4f, Settings.m_screenHeight * 0.7f));
            players[0].m_vehicleChoice = new Sprite(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.33f, Settings.m_screenHeight * 0.65f), 0, Vector2.One);
                // Player 2 -- BOTTOM_RIGHT
            players[1].m_choice1Button = new Button(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.9f, Settings.m_screenHeight * 0.7f));
            players[1].m_choice2Button = new Button(TextureHandler.vehicleLargeTractorWhite, new Vector2(Settings.m_screenWidth * 0.8f, Settings.m_screenHeight * 0.7f));
            players[1].m_choice3Button = new Button(TextureHandler.vehicleLargeTankWhite, new Vector2(Settings.m_screenWidth * 0.7f, Settings.m_screenHeight * 0.7f));
            players[1].m_choice4Button = new Button(TextureHandler.vehicleLargeBuggyWhite, new Vector2(Settings.m_screenWidth * 0.6f, Settings.m_screenHeight * 0.7f));
			players[1].m_vehicleChoice = new Sprite(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.67f, Settings.m_screenHeight * 0.65f), 0, Vector2.One);
                // Player 3 -- TOP_LEFT
            players[2].m_choice1Button = new Button(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.1f, Settings.m_screenHeight * 0.3f));
            players[2].m_choice2Button = new Button(TextureHandler.vehicleLargeTractorWhite, new Vector2(Settings.m_screenWidth * 0.2f, Settings.m_screenHeight * 0.3f));
            players[2].m_choice3Button = new Button(TextureHandler.vehicleLargeTankWhite, new Vector2(Settings.m_screenWidth * 0.3f, Settings.m_screenHeight * 0.3f));
            players[2].m_choice4Button = new Button(TextureHandler.vehicleLargeBuggyWhite, new Vector2(Settings.m_screenWidth * 0.4f, Settings.m_screenHeight * 0.3f));
			players[2].m_vehicleChoice = new Sprite(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.33f, Settings.m_screenHeight * 0.35f), 180, Vector2.One);
			players[2].m_choice1Button.m_sprite.SetRotationDegrees(180);
			players[2].m_choice2Button.m_sprite.SetRotationDegrees(180);
			players[2].m_choice3Button.m_sprite.SetRotationDegrees(180);
			players[2].m_choice4Button.m_sprite.SetRotationDegrees(180);
			// Player 4 -- TOP_RIGHT
            players[3].m_choice1Button = new Button(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.9f, Settings.m_screenHeight * 0.3f));
            players[3].m_choice2Button = new Button(TextureHandler.vehicleLargeTractorWhite, new Vector2(Settings.m_screenWidth * 0.8f, Settings.m_screenHeight * 0.3f));
            players[3].m_choice3Button = new Button(TextureHandler.vehicleLargeTankWhite, new Vector2(Settings.m_screenWidth * 0.7f, Settings.m_screenHeight * 0.3f));
            players[3].m_choice4Button = new Button(TextureHandler.vehicleLargeBuggyWhite, new Vector2(Settings.m_screenWidth * 0.6f, Settings.m_screenHeight * 0.3f));
			players[3].m_vehicleChoice = new Sprite(TextureHandler.vehicleLargeCarWhite, new Vector2(Settings.m_screenWidth * 0.67f, Settings.m_screenHeight * 0.35f), 180, Vector2.One);
			players[3].m_choice1Button.m_sprite.SetRotationDegrees(180);
			players[3].m_choice2Button.m_sprite.SetRotationDegrees(180);
			players[3].m_choice3Button.m_sprite.SetRotationDegrees(180);
			players[3].m_choice4Button.m_sprite.SetRotationDegrees(180);

                // Misc
            m_playerBackButton = new Button(TextureHandler.backButton, new Vector2(Settings.m_screenWidth * 0.45f, Settings.m_screenHeight * 0.5f));
            m_playerGoButton = new Button(TextureHandler.startButton, new Vector2(Settings.m_screenWidth * 0.55f, Settings.m_screenHeight * 0.5f));


			// Play song
			//MediaPlayer.Play(AudioHandler.menuMusic);
			//MediaPlayer.IsRepeating = true;


			// Set menu screen
			m_currentScreen = MenuScreenState.MAIN_MENU;

            m_touchState = TouchState.IDLE;

			// Set initial next state
			m_nextState = GameState.IN_GAME;

			// Change exectution state
			m_currentExecutionState = ExecutionState.RUNNING;
		}

		public override void Update(ref TouchHandler touchHandler_, GameTime gameTime_) {
			// Update menu state
			// ================

            // NOTE: Debug Purposes
            if (Keyboard.GetState().IsKeyDown(Keys.Q)) {
                m_currentScreen = MenuScreenState.MAIN_MENU;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                m_currentScreen = MenuScreenState.PLAYER_SELECT;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R)) {
                m_currentScreen = MenuScreenState.OPTIONS;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.T)) {
                m_currentScreen = MenuScreenState.CREDITS;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y)) {
                bool ready = true;
                int readyCount = 0;

                // Loop through each player
                for (int i = 0; i < players.Count; i++)
                {

                    if (players[i].m_playerState == PlayerState.JOINED || players[i].m_playerState == PlayerState.VEHICLE_SELECTED)
                    {
                        ready = false;
                        
                    }
                    else if (players[i].m_playerState == PlayerState.READY)
                    {
                        readyCount++;
                    }
                }
                Settings.m_numberOfPlayers = readyCount;
                m_currentExecutionState = ExecutionState.CHANGING;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_currentScreen = MenuScreenState.CONTROLS;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_nextState = GameState.VICTORY_SCREEN;
                m_currentExecutionState = ExecutionState.CHANGING;
            }
              
            // Change player count with keys - TEMP
            if (Keyboard.GetState().IsKeyDown(Keys.D1)) {
                Settings.m_numberOfPlayers = 1;
            } else if (Keyboard.GetState().IsKeyDown(Keys.D2)) {
                Settings.m_numberOfPlayers = 2;
            } else if (Keyboard.GetState().IsKeyDown(Keys.D3)) {
                Settings.m_numberOfPlayers = 3;
            } else if (Keyboard.GetState().IsKeyDown(Keys.D4)) {
                Settings.m_numberOfPlayers = 4;
            }

            

            Debug.AddText("7 for white cow, 8 for highland", new Vector2(20, 850));
            Debug.AddText("C for car, B for tractor, V for tank", new Vector2(20, 900));

            // NOTE: Debug keys
            if (Keyboard.GetState().IsKeyDown(Keys.M)) {
                // Add player to game
                players[0].m_playerState = PlayerState.JOINED;
                players[0].m_actionButton.m_sprite.SetTexture(TextureHandler.readyUnselectedButton);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) {
                players[0].m_playerState = PlayerState.VEHICLE_SELECTED;
                players[0].m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeCarBlue);
				TextureHandler.player1Vehicle = TextureHandler.vehicleSmallCarBlue;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3)) {
                players[0].m_playerState = PlayerState.READY;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7)) {
                players[1].m_playerState = PlayerState.READY;
                Settings.m_joinedPlayers[1] = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8)) {
                players[2].m_playerState = PlayerState.READY;
                Settings.m_joinedPlayers[2] = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9)) {
                players[3].m_playerState = PlayerState.READY;
                Settings.m_joinedPlayers[3] = true;
            }

			// Update touch input handler
			touchHandler_.Update();

            // If there is a touch on the screen
            if (touchHandler_.GetTouches().Count > 0) {
                // Get the position of the last touch 
                m_touchState = TouchState.TOUCHING;
                m_lastPosition = touchHandler_.GetTouches()[touchHandler_.GetTouches().Count - 1].Position;
            }

            for (int i = 0; i < players.Count; i++)
            {
                // If the player has joined
                if (players[i].m_playerState == PlayerState.JOINED)
                {
                    Settings.m_joinedPlayers[i] = true;
                }
            }

                // If there are no touches on the screen (finger released button)
            if (touchHandler_.GetTouches().Count == 0 && m_touchState == TouchState.TOUCHING)
            {
                switch (m_currentScreen)
                {
                    case MenuScreenState.MAIN_MENU:
                        // Main Menu screen

                        if (m_playButton.Activated(m_lastPosition))
                        {
                            // Go to Player Select
                            m_currentScreen = MenuScreenState.PLAYER_SELECT;
                        }
                        if (m_exitButton.Activated(m_lastPosition))
                        {
                            // Close app
                        }
                        if (m_creditsButton.Activated(m_lastPosition))
                        {
                            // Go to Credits
                            m_currentScreen = MenuScreenState.CREDITS;
                        }
                        if (m_controlsButton.Activated(m_lastPosition))
                        {
                            // Go to control scheme screen
                            m_currentScreen = MenuScreenState.CONTROLS;
                        }
                        break;
                    case MenuScreenState.PLAYER_SELECT:
                        // Player Select screen

                        //counter for amount of players
                        //so we can assign the proper texture                    

                        // Loop logic for each player
                        foreach (PlayerSelectStruct pss in players)
                        {


                            // Check player's join state
                            if (pss.m_playerState == PlayerState.NONE)
                            {
                                // If action button has been pressed
                                if (pss.m_actionButton.Activated(m_lastPosition))
                                {
                                    // Add player to game
                                    pss.m_playerState = PlayerState.JOINED;
                                    pss.m_actionButton.m_sprite.SetTexture(TextureHandler.readyUnselectedButton);
                                }
                            }
                            else if (pss.m_playerState == PlayerState.JOINED)
                            {
                                // If vehicle button is pressed
                                if (pss.m_choice1Button.Activated(m_lastPosition))
                                {
                                    // TODO: Check if the vehicle has already been chosen
                                    pss.m_playerState = PlayerState.VEHICLE_SELECTED;

                                    #region PickCorrectColour

                                    if (pss.ID == 0)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeCarBlue);
										TextureHandler.player1Vehicle = TextureHandler.vehicleSmallCarBlue;
                                    }
                                    else if (pss.ID == 1)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeCarOrange);
										TextureHandler.player2Vehicle = TextureHandler.vehicleSmallCarOrange;
                                    }
                                    else if (pss.ID == 2)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeCarPurple);
										TextureHandler.player3Vehicle = TextureHandler.vehicleSmallCarPurple;
                                    }
                                    else if (pss.ID == 3)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeCarYellow);
										TextureHandler.player4Vehicle = TextureHandler.vehicleSmallCarYellow;
                                    }

                                    #endregion

                                }
                                else if (pss.m_choice2Button.Activated(m_lastPosition))
                                {
                                    // TODO: Check if the vehicle has already been chosen
                                    pss.m_playerState = PlayerState.VEHICLE_SELECTED;

                                    #region PickCorrectColour

                                    if (pss.ID == 0)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTractorBlue);
										TextureHandler.player1Vehicle = TextureHandler.vehicleSmallTractorBlue;
                                    }
                                    else if (pss.ID == 1)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTractorOrange);
										TextureHandler.player2Vehicle = TextureHandler.vehicleSmallTractorOrange;
                                    }
                                    else if (pss.ID == 2)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTractorPurple);
										TextureHandler.player3Vehicle = TextureHandler.vehicleSmallTractorPurple;
                                    }
                                    else if (pss.ID == 3)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTractorGreen);
										TextureHandler.player4Vehicle = TextureHandler.vehicleSmallTractorGreen;
                                    }

                                    #endregion

                                }
                                else if (pss.m_choice3Button.Activated(m_lastPosition))
                                {
                                    // TODO: Check if the vehicle has already been chosen
                                    pss.m_playerState = PlayerState.VEHICLE_SELECTED;

                                    #region PickCorrectColour

                                    if (pss.ID == 0)
                                    {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTankBlue);
										TextureHandler.player1Vehicle = TextureHandler.vehicleSmallTankBlue;
                                    }
                                    else if (pss.ID == 1)
                                    {
										pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTankOrange);
										TextureHandler.player2Vehicle = TextureHandler.vehicleSmallTankOrange;
                                    }
                                    else if (pss.ID == 2)
                                    {
										pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTankPurple);
										TextureHandler.player3Vehicle = TextureHandler.vehicleSmallTankPurple;
                                    }
                                    else if (pss.ID == 3)
                                    {
										pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeTankGreen);
										TextureHandler.player4Vehicle = TextureHandler.vehicleSmallTankGreen;
                                    }

                                    #endregion

                                }
                                else if (pss.m_choice4Button.Activated(m_lastPosition))
                                {
                                    // TODO: Check if the vehicle has already been chosen
                                    pss.m_playerState = PlayerState.VEHICLE_SELECTED;

                                    if (pss.ID == 0) {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeBuggyBlue);
										TextureHandler.player1Vehicle = TextureHandler.vehicleSmallBuggyBlue;
                                    } else if (pss.ID == 1) {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeBuggyOrange);
										TextureHandler.player2Vehicle = TextureHandler.vehicleSmallBuggyOrange;
                                    } else if (pss.ID == 2) {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeBuggyPurple);
										TextureHandler.player3Vehicle = TextureHandler.vehicleSmallBuggyPurple;
                                    } else if (pss.ID == 3) {
                                        pss.m_vehicleChoice.SetTexture(TextureHandler.vehicleLargeBuggyYellow);
										TextureHandler.player4Vehicle = TextureHandler.vehicleSmallBuggyYellow;
                                    }
                                }
                            }
                            else if (pss.m_playerState == PlayerState.VEHICLE_SELECTED)
                            {
                                if (pss.m_actionButton.Activated(m_lastPosition))
                                {
									pss.m_actionButton.m_sprite.SetTexture(TextureHandler.readySelectedButton);
                                    pss.m_playerState = PlayerState.READY;
                                }
                            }
                        }

                        // Check if all players are ready
                        if (m_playerBackButton.Activated(m_lastPosition))
                        {

                            /*// Check if players are ready
                            if (ready && readyCount > 0) {
                                Settings.m_numberOfPlayers = readyCount;
                                m_currentExecutionState = ExecutionState.CHANGING;
                            }*/
                        }
						if(m_playerGoButton.Activated(m_lastPosition)) {
							bool ready = true;
							int readyCount = 0;

							// Loop through each player
							for(int i = 0; i < players.Count; i++) {

								if(players[i].m_playerState == PlayerState.JOINED || players[i].m_playerState == PlayerState.VEHICLE_SELECTED) {
									ready = false;
									// If the player has joined
									if(players[i].m_playerState == PlayerState.JOINED) {
										Settings.m_joinedPlayers[i] = true;
									}
								} else if(players[i].m_playerState == PlayerState.READY) {
									readyCount++;
								}
							}
							if(ready && readyCount > 0) {
								MediaPlayer.Stop();
								Settings.m_numberOfPlayers = readyCount;
								m_currentExecutionState = ExecutionState.CHANGING;
							}
						}

                        break;
                    case MenuScreenState.OPTIONS:
                        // Options screen

                        if (m_backButton.Activated(m_lastPosition))
                        {
                            // Go back to Main Menu
                            m_currentScreen = MenuScreenState.MAIN_MENU;
                        }
                        break;
                    case MenuScreenState.CONTROLS:
                        // Controls Screen
                        if (m_backButton.Activated(m_lastPosition))
                        {
                            // Go back to the main Menu
                            m_currentScreen = MenuScreenState.MAIN_MENU;
                        }
                        break;
                    case MenuScreenState.CREDITS:
                        // Credits screen

                        if (m_MenuButton.Activated(m_lastPosition))
                        {
                            // Go back to Main Menu
                            m_currentScreen = MenuScreenState.MAIN_MENU;
                        }
                        break;
                }
				m_touchState = TouchState.IDLE;
            }
		}

		public override void Draw(GraphicsDevice graphicsDevice_) {
			// Render objects to the screen
			// ================
			
			// Clear the screen
            graphicsDevice_.Clear(Color.LawnGreen);

            // Render graphics
            GraphicsHandler.StartDrawing();

            // Draw sprites
            GraphicsHandler.DrawSprite(m_background);
            GraphicsHandler.DrawSprite(m_teamLogo);
            //GraphicsHandler.DrawSprite(m_gameLogo);

            // Draw buttons
            switch (m_currentScreen) {
                case MenuScreenState.MAIN_MENU:
                    GraphicsHandler.DrawSprite(m_playButton.m_sprite);
                    GraphicsHandler.DrawSprite(m_exitButton.m_sprite);
                    GraphicsHandler.DrawSprite(m_creditsButton.m_sprite);
                    GraphicsHandler.DrawSprite(m_controlsButton.m_sprite);
                    break;
                case MenuScreenState.PLAYER_SELECT:
                    GraphicsHandler.DrawText("Player Selection", new Vector2(Settings.m_screenWidth * 0.50f, Settings.m_screenHeight * 0.50f), Color.Black);
                    GraphicsHandler.DrawText("Number of Players: " + Settings.m_numberOfPlayers.ToString(), new Vector2(Settings.m_screenWidth * 0.475f, Settings.m_screenHeight * 0.5625f), Color.Black);
                    
                    // Loop each player and render the relevant information
                    foreach (PlayerSelectStruct pss in players) {
                        if (pss.m_playerState == PlayerState.NONE || pss.m_playerState == PlayerState.VEHICLE_SELECTED || pss.m_playerState == PlayerState.READY) {
                            GraphicsHandler.DrawSprite(pss.m_actionButton.m_sprite);
                        }
                        if (pss.m_playerState == PlayerState.JOINED) {
                            GraphicsHandler.DrawSprite(pss.m_choice1Button.m_sprite);
                            GraphicsHandler.DrawSprite(pss.m_choice2Button.m_sprite);
                            GraphicsHandler.DrawSprite(pss.m_choice3Button.m_sprite);
                            GraphicsHandler.DrawSprite(pss.m_choice4Button.m_sprite);
                        } else if (pss.m_playerState == PlayerState.VEHICLE_SELECTED || pss.m_playerState == PlayerState.READY) {
                            GraphicsHandler.DrawSprite(pss.m_vehicleChoice);
                        }
                    }

                    // TODO: Render "back" and "go" buttons
                    GraphicsHandler.DrawSprite(m_playerBackButton.m_sprite);
                    GraphicsHandler.DrawSprite(m_playerGoButton.m_sprite);














                    break;
                case MenuScreenState.OPTIONS:
                    // Insert buttons for options
                    GraphicsHandler.DrawText("Options", new Vector2(Settings.m_screenWidth / 2, Settings.m_screenHeight / 2), Color.Black);
                    GraphicsHandler.DrawSprite(m_MenuButton.m_sprite);
                    break;
                case MenuScreenState.CONTROLS:
                    // Insert control scheme layout
                    GraphicsHandler.DrawText("Controls", new Vector2(Settings.m_screenWidth / 2, Settings.m_screenHeight / 2), Color.Black);
                    GraphicsHandler.DrawSprite(m_control_scheme);
                    GraphicsHandler.DrawSprite(m_MenuButton.m_sprite);
                    break;
                case MenuScreenState.CREDITS:
                    // Insert text for credits
                    GraphicsHandler.DrawText("Team Leader: Dean Sinclair", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f), Color.Black);
                    GraphicsHandler.DrawText("Producer: Dwyer McNally", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 25), Color.Black);
                    GraphicsHandler.DrawText("Programmers:", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 50), Color.Black);
                    GraphicsHandler.DrawText("            Daniel Divers", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 70), Color.Black);
                    GraphicsHandler.DrawText("            Cameron Fleming:", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 90), Color.Black);
                    GraphicsHandler.DrawText("            Nathan Headley", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 110), Color.Black);
                    GraphicsHandler.DrawText("            Dean Sinclair", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 130), Color.Black);
                    GraphicsHandler.DrawText("            Cemre Tekpinar", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 150), Color.Black);
                    GraphicsHandler.DrawText("Game Design: Dwyer McNally", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 175), Color.Black);
                    GraphicsHandler.DrawText("Art: Gillian Annandale", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 200), Color.Black);
                    GraphicsHandler.DrawText("Audio: Russell Ferguson", new Vector2(Settings.m_screenWidth / 2 - 100, Settings.m_screenHeight * 0.4f + 225), Color.Black);
                    GraphicsHandler.DrawSprite(m_MenuButton.m_sprite);
                    break;
            }
            

            // Draw animated sprites
            foreach (AnimatedSprite anim_ in m_animatedSprites) {
                if (anim_.IsVisible()) {
                    GraphicsHandler.DrawAnimatedSprite(anim_);
                }
            }

            // Draw particles
            foreach (Particle part_ in m_particles) {
                if (part_.GetLife() > 0) {
                    //GraphicsHandler.DrawParticle(/*texture,*/ part_.GetPosition(), Color.White);
                }
            }

            // Stop rendering
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

	enum MenuScreenState {
		// Enum for each type of menu screen
		// ================
		MAIN_MENU,
		PLAYER_SELECT,
		OPTIONS,
        CONTROLS,
		CREDITS
	}

    enum TouchState {
        // Enum for each touch state
        // ================
        IDLE,
        TOUCHING,
        RELEASED
    }

    enum PlayerState {
        NONE,
        JOINED,
        VEHICLE_SELECTED,
        READY
    }

    class PlayerSelectStruct {
        public Button m_actionButton;
        public Button m_choice1Button;
        public Button m_choice2Button;
        public Button m_choice3Button;
        public Button m_choice4Button;
        public Sprite m_vehicleChoice;            // Change to sprite

        public PlayerState m_playerState;
        public Texture2D playerChoice;

        public int ID;
    }
}
