using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

namespace EverybodyVotesChannel
{
	public class Game1 : Game
	{
		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Color[] _buffer;

		private readonly int WINDOW_WIDTH, WINDOW_HEIGHT;

		private readonly int BUTTON_SIZE;

		private Texture2D _red;        // A1
		private Texture2D _red_off;    // A1
		private Texture2D _blue;       // A2
		private Texture2D _blue_off;   // A2
		private Texture2D _green;      // A3
		private Texture2D _green_off;  // A3
		private Texture2D _white;      // A4
		private Texture2D _white_off;  // A4

		private Texture2D _purple;     // B1-B4 for player 1
		private Texture2D _purple_off; // B1-B4 for player 1
		private Texture2D _yellow;     // B1-B4 for player 2
		private Texture2D _yellow_off; // B1-B4 for player 2

		private Texture2D _black;      // Menu buttons
		private Texture2D _black_off;  // Menu buttons

		private Texture2D _area_p1;    // Area to contain the stick indicator for player 1
		private Texture2D _area_p2;	   // Area to contain the stick indicator for player 2
		private Texture2D _stick1;     // Stick indicator for player 1
		private Texture2D _stick2;     // Stick indicator for player 2

		private Dictionary<string, int> _votes;
		private SpriteFont _font;

		private string _message = "";

		/// <summary>
		/// Game constructor
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
			
			// Get screen width and height
			int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			int maxWidthFromHeight = (int) (screenHeight * 9f / 21f);
			int maxHeightFromWidth = (int) (screenWidth * 21f / 9f);
			WINDOW_WIDTH = Math.Min(screenWidth, maxWidthFromHeight);
			WINDOW_HEIGHT = Math.Min(screenHeight, maxHeightFromWidth);
			BUTTON_SIZE = (int) ((float)WINDOW_WIDTH / 11);
		}

		/// <summary>
		/// Does any setup prior to the first frame that doesn't need loaded content.
		/// </summary>
		protected override void Initialize()
		{
			Input.Initialize(); // Sets up the input library
			Persistence.InitForceRemote();
			Persistence.SetLocalPath("/home/joe/.devcade/EverybodyVotesChannel/");
			
			_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			
			_graphics.ApplyChanges();
			
			base.Initialize();
		}

		/// <summary>
		/// Does any setup prior to the first frame that needs loaded content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_votes = Persistence.LoadSync<Dictionary<string, int>>("global", "votes", null);
			
			// Load _font as builtin font
			_font = Content.Load<SpriteFont>("Font");

			_votes ??= new Dictionary<string, int> {
				["Player 1 A1"] = 0,
				["Player 1 A2"] = 0,
				["Player 1 A3"] = 0,
				["Player 1 A4"] = 0,
				["Player 2 A1"] = 0,
				["Player 2 A2"] = 0,
				["Player 2 A3"] = 0,
				["Player 2 A4"] = 0,
				["Player 1 B1"] = 0,
				["Player 1 B2"] = 0,
				["Player 1 B3"] = 0,
				["Player 1 B4"] = 0,
				["Player 2 B1"] = 0,
				["Player 2 B2"] = 0,
				["Player 2 B3"] = 0,
				["Player 2 B4"] = 0,
				["Player 1 Menu"] = 0,
				["Player 2 Menu"] = 0,
                                ["Player 1 StickUp"] = 0,
                                ["Player 2 StickUp"] = 0,
                                ["Player 1 StickDown"] = 0,
                                ["Player 2 StickDown"] = 0,
                                ["Player 1 StickRight"] = 0,
                                ["Player 2 StickRight"] = 0,
                                ["Player 1 StickLeft"] = 0,
                                ["Player 2 StickLeft"] = 0,
                                ["Player 1 StickUpLeft"] = 0,
                                ["Player 2 StickUpLeft"] = 0,
                                ["Player 1 StickUpRight"] = 0,
                                ["Player 2 StickUpRight"] = 0,
                                ["Player 1 StickDownLeft"] = 0,
                                ["Player 2 StickDownLeft"] = 0,
                                ["Player 1 StickDownRight"] = 0,
                                ["Player 2 StickDownRight"] = 0,
			};
			
			var red = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var red_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var blue = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var blue_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var green = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var green_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var white = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var white_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var purple = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var purple_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var yellow = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var yellow_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var black = new Color[BUTTON_SIZE * BUTTON_SIZE];
			var black_off = new Color[BUTTON_SIZE * BUTTON_SIZE];
			
			var area_p1 = new Color[BUTTON_SIZE * BUTTON_SIZE * 4];
			var area_p2 = new Color[BUTTON_SIZE * BUTTON_SIZE * 4];
			var stick1 = new Color[(BUTTON_SIZE / 2) * (BUTTON_SIZE / 2)];
			var stick2 = new Color[(BUTTON_SIZE / 2) * (BUTTON_SIZE / 2)];
			
			for (int i = 0; i < red.Length; i++)
			{
				red[i] = Color.Red;
				red_off[i] = Color.DarkRed;
				blue[i] = Color.Blue;
				blue_off[i] = Color.DarkBlue;
				green[i] = Color.Green;
				green_off[i] = Color.DarkGreen;
				white[i] = Color.White;
				white_off[i] = Color.Gray;
				purple[i] = Color.DarkOrchid;
				purple_off[i] = Color.Purple;
				yellow[i] = Color.Gold;
				yellow_off[i] = Color.Goldenrod;
				black[i] = Color.DarkSlateGray;
				black_off[i] = Color.Black;
			}

			float ratio = 0.72f;
			
			Color c_area_p1 = new Color(
				Color.CornflowerBlue.R * ratio / 255 + Color.DarkOrchid.R * (1 - ratio) / 255,
				Color.CornflowerBlue.G * ratio / 255 + Color.DarkOrchid.G * (1 - ratio) / 255,
				Color.CornflowerBlue.B * ratio / 255 + Color.DarkOrchid.B * (1 - ratio) / 255,
				Color.CornflowerBlue.A * ratio / 255 + Color.DarkOrchid.A * (1 - ratio) / 255);
			
			Color c_area_p2 = new Color(
				Color.CornflowerBlue.R * ratio / 255 + Color.Gold.R * (1 - ratio) / 255,
				Color.CornflowerBlue.G * ratio / 255 + Color.Gold.G * (1 - ratio) / 255,
				Color.CornflowerBlue.B * ratio / 255 + Color.Gold.B * (1 - ratio) / 255,
				Color.CornflowerBlue.A * ratio / 255 + Color.Gold.A * (1 - ratio) / 255);
			
			for (int i = 0; i < area_p1.Length; i++)
			{
				// area is slightly darker than cornflower blue
				area_p1[i] = c_area_p1;
				area_p2[i] = c_area_p2;
			}
			
			for (int i = 0; i < stick1.Length; i++)
			{
				stick1[i] = Color.DarkOrchid;
				stick2[i] = Color.Gold;
			}
			
			_red = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_red.SetData(red);
			_red_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_red_off.SetData(red_off);
			
			_blue = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_blue.SetData(blue);
			_blue_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_blue_off.SetData(blue_off);
			
			_green = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_green.SetData(green);
			_green_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_green_off.SetData(green_off);
			
			_white = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_white.SetData(white);
			_white_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_white_off.SetData(white_off);
			
			_purple = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_purple.SetData(purple);
			_purple_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_purple_off.SetData(purple_off);
			
			_yellow = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_yellow.SetData(yellow);
			_yellow_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_yellow_off.SetData(yellow_off);
			
			_black = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_black.SetData(black);
			_black_off = new Texture2D(GraphicsDevice, BUTTON_SIZE, BUTTON_SIZE);
			_black_off.SetData(black_off);
			
			_area_p1 = new Texture2D(GraphicsDevice, BUTTON_SIZE * 2, BUTTON_SIZE * 2);
			_area_p1.SetData(area_p1);
			_area_p2 = new Texture2D(GraphicsDevice, BUTTON_SIZE * 2, BUTTON_SIZE * 2);
			_area_p2.SetData(area_p2);
			
			_stick1 = new Texture2D(GraphicsDevice, BUTTON_SIZE / 2, BUTTON_SIZE / 2);
			_stick1.SetData(stick1);
			_stick2 = new Texture2D(GraphicsDevice, BUTTON_SIZE / 2, BUTTON_SIZE / 2);
			_stick2.SetData(stick2);
		}

		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			Input.Update(); // Updates the state of the input library

			KeyboardState k = Keyboard.GetState();
			
			// Exit when both menu buttons are pressed (or escape for keyboard debuging)
			// You can change this but it is suggested to keep the keybind of both menu
			// buttons at once for gracefull exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Quit();
			}

			if (Input.GetButtonDown(1, Input.ArcadeButtons.A1) || k.IsKeyDown(Keys.Q)) {
				_votes["Player 1 A1"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.A2) || k.IsKeyDown(Keys.W)) {
				_votes["Player 1 A2"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.A3) || k.IsKeyDown(Keys.E)) {
				_votes["Player 1 A3"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.A4) || k.IsKeyDown(Keys.R)) {
				_votes["Player 1 A4"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.B1) || k.IsKeyDown(Keys.A)) {
				_votes["Player 1 B1"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.B2) || k.IsKeyDown(Keys.S)) {
				_votes["Player 1 B2"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.B3) || k.IsKeyDown(Keys.D)) {
				_votes["Player 1 B3"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.B4) || k.IsKeyDown(Keys.F)) {
				_votes["Player 1 B4"]++;
			}
			if (Input.GetButtonDown(1, Input.ArcadeButtons.Menu) || k.IsKeyDown(Keys.G)) {
				_votes["Player 1 Menu"]++;
			}
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickUp) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 1 StickUp"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 1 StickDown"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickRight) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickUp)) {
                                _votes["Player 1 StickRight"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(1, Input.ArcadeButtons.StickUp)) {
                                _votes["Player 1 StickLeft"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickUp) && Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft)) {
                                _votes["Player 1 StickUpLeft"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickUp) && Input.GetButtonDown(1, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 1 StickUpRight"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickDown) && Input.GetButtonDown(1, Input.ArcadeButtons.StickLeft)) {
                                _votes["Player 1 StickDownLeft"]++;
                        }
                        if (Input.GetButtonDown(1, Input.ArcadeButtons.StickDown) && Input.GetButtonDown(1, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 1 StickDownRight"]++;
                        }
			
			if (Input.GetButtonDown(2, Input.ArcadeButtons.A1) || k.IsKeyDown(Keys.U)) {
				_votes["Player 2 A1"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.A2) || k.IsKeyDown(Keys.I)) {
				_votes["Player 2 A2"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.A3) || k.IsKeyDown(Keys.O)) {
				_votes["Player 2 A3"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.A4) || k.IsKeyDown(Keys.P)) {
				_votes["Player 2 A4"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.B1) || k.IsKeyDown(Keys.J)) {
				_votes["Player 2 B1"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.B2) || k.IsKeyDown(Keys.K)) {
				_votes["Player 2 B2"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.B3) || k.IsKeyDown(Keys.L)) {
				_votes["Player 2 B3"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.B4) || k.IsKeyDown(Keys.OemSemicolon)) {
				_votes["Player 2 B4"]++;
			}
			if (Input.GetButtonDown(2, Input.ArcadeButtons.Menu) || k.IsKeyDown(Keys.H)) {
				_votes["Player 2 Menu"]++;
			}
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickUp) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 2 StickUp"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 2 StickDown"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickRight) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickUp)) {
                                _votes["Player 2 StickRight"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickLeft) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickDown) && !Input.GetButtonDown(2, Input.ArcadeButtons.StickUp)) {
                                _votes["Player 2 StickLeft"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickUp) && Input.GetButtonDown(2, Input.ArcadeButtons.StickLeft)) {
                                _votes["Player 2 StickUpLeft"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickUp) && Input.GetButtonDown(2, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 2 StickUpRight"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickDown) && Input.GetButtonDown(2, Input.ArcadeButtons.StickLeft)) {
                                _votes["Player 2 StickDownLeft"]++;
                        }
                        if (Input.GetButtonDown(2, Input.ArcadeButtons.StickDown) && Input.GetButtonDown(2, Input.ArcadeButtons.StickRight)) {
                                _votes["Player 2 StickDownRight"]++;
                        }
			
			var max = _votes.MaxBy(x => x.Value);

			_message = max.Value > 0 ? $"Button {max.Key} is winning with {max.Value} votes!" : "No votes yet!";

			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			
			Point center = new (WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);
			
			// Draw the message in the center of the screen
			
			_spriteBatch.DrawString(_font, _message, new Vector2(center.X - _font.MeasureString(_message).X / 2, center.Y - _font.MeasureString(_message).Y / 2 - BUTTON_SIZE * 5), Color.Black);

			KeyboardState state = Keyboard.GetState();

			#region Player 1
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.A1) || state.IsKeyDown(Keys.Q) ? _red : _red_off,
				new Rectangle(center.X - BUTTON_SIZE * 5, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.A2) || state.IsKeyDown(Keys.W) ? _blue : _blue_off,
				new Rectangle(center.X - BUTTON_SIZE * 4, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.A3) || state.IsKeyDown(Keys.E) ? _green : _green_off,
				new Rectangle(center.X - BUTTON_SIZE * 3, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.A4) || state.IsKeyDown(Keys.R) ? _white : _white_off,
				new Rectangle(center.X - BUTTON_SIZE * 2, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.B1) || state.IsKeyDown(Keys.A) ? _purple : _purple_off,
				new Rectangle(center.X - BUTTON_SIZE * 5, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.B2) || state.IsKeyDown(Keys.S) ? _purple : _purple_off,
				new Rectangle(center.X - BUTTON_SIZE * 4, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.B3) || state.IsKeyDown(Keys.D) ? _purple : _purple_off,
				new Rectangle(center.X - BUTTON_SIZE * 3, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.B4) || state.IsKeyDown(Keys.F) ? _purple : _purple_off,
				new Rectangle(center.X - BUTTON_SIZE * 2, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);

			_spriteBatch.Draw(Input.GetButton(1, Input.ArcadeButtons.Menu) || state.IsKeyDown(Keys.G) ? _black : _black_off,
				new Rectangle(center.X - BUTTON_SIZE, (int) (center.Y - BUTTON_SIZE * 0.5), BUTTON_SIZE, BUTTON_SIZE),
				Color.White);
			
			#endregion
			
			#region Player 2
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.A1) || state.IsKeyDown(Keys.U) ? _red : _red_off,
				new Rectangle(center.X + BUTTON_SIZE, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.A2) || state.IsKeyDown(Keys.I) ? _blue : _blue_off,
				new Rectangle(center.X + BUTTON_SIZE * 2, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.A3) || state.IsKeyDown(Keys.O) ? _green : _green_off,
				new Rectangle(center.X + BUTTON_SIZE * 3, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.A4) || state.IsKeyDown(Keys.P) ? _white : _white_off,
				new Rectangle(center.X + BUTTON_SIZE * 4, center.Y - BUTTON_SIZE, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.B1) || state.IsKeyDown(Keys.J) ? _yellow : _yellow_off,
				new Rectangle(center.X + BUTTON_SIZE, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.B2) || state.IsKeyDown(Keys.K) ? _yellow : _yellow_off,
				new Rectangle(center.X + BUTTON_SIZE * 2, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.B3) || state.IsKeyDown(Keys.L) ? _yellow : _yellow_off,
				new Rectangle(center.X + BUTTON_SIZE * 3, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			_spriteBatch.Draw(Input.GetButton(2, Input.ArcadeButtons.B4) || state.IsKeyDown(Keys.OemSemicolon) ? _yellow : _yellow_off,
				new Rectangle(center.X + BUTTON_SIZE * 4, center.Y, BUTTON_SIZE, BUTTON_SIZE), Color.White);

			_spriteBatch.Draw(
				Input.GetButton(2, Input.ArcadeButtons.Menu) || state.IsKeyDown(Keys.H) ? _black : _black_off,
				new Rectangle(center.X, (int)(center.Y - BUTTON_SIZE * 0.5), BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			#endregion
			
			// draw vote counts over each button
			_spriteBatch.DrawString(_font, _votes["Player 1 A1"].ToString(), new Vector2(center.X - BUTTON_SIZE * 5, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 A2"].ToString(), new Vector2(center.X - BUTTON_SIZE * 4, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 A3"].ToString(), new Vector2(center.X - BUTTON_SIZE * 3, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 A4"].ToString(), new Vector2(center.X - BUTTON_SIZE * 2, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 B1"].ToString(), new Vector2(center.X - BUTTON_SIZE * 5, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 B2"].ToString(), new Vector2(center.X - BUTTON_SIZE * 4, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 B3"].ToString(), new Vector2(center.X - BUTTON_SIZE * 3, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 B4"].ToString(), new Vector2(center.X - BUTTON_SIZE * 2, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 1 Menu"].ToString(), new Vector2(center.X - BUTTON_SIZE, center.Y - BUTTON_SIZE * 0.5f), Color.White);

                        _spriteBatch.DrawString(_font, _votes["Player 1 StickUp"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 3), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickDown"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 3), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickLeft"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 3)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickRight"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 3)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickUpLeft"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickUpRight"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickDownLeft"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 1 StickDownRight"].ToString(), new Vector2(center.X - (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);
			
			_spriteBatch.DrawString(_font, _votes["Player 2 A1"].ToString(), new Vector2(center.X + BUTTON_SIZE, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 A2"].ToString(), new Vector2(center.X + BUTTON_SIZE * 2, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 A3"].ToString(), new Vector2(center.X + BUTTON_SIZE * 3, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 A4"].ToString(), new Vector2(center.X + BUTTON_SIZE * 4, center.Y - BUTTON_SIZE), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 B1"].ToString(), new Vector2(center.X + BUTTON_SIZE, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 B2"].ToString(), new Vector2(center.X + BUTTON_SIZE * 2, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 B3"].ToString(), new Vector2(center.X + BUTTON_SIZE * 3, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 B4"].ToString(), new Vector2(center.X + BUTTON_SIZE * 4, center.Y), Color.White);
			_spriteBatch.DrawString(_font, _votes["Player 2 Menu"].ToString(), new Vector2(center.X, center.Y - BUTTON_SIZE * 0.5f), Color.White);
			
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickUp"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 3), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickDown"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 3), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickLeft"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 3)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickRight"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 3)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickUpLeft"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickUpRight"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 4.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickDownLeft"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 1.5), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);
                        _spriteBatch.DrawString(_font, _votes["Player 2 StickDownRight"].ToString(), new Vector2(center.X + (int) (BUTTON_SIZE * 4.5), center.Y - (int) (BUTTON_SIZE * 1.5)), Color.White);

			#region RANDY YOUR STICKS

			_spriteBatch.Draw(_area_p1,
				new Rectangle(center.X - BUTTON_SIZE * 4, center.Y - BUTTON_SIZE * 4, BUTTON_SIZE * 2, BUTTON_SIZE * 2),
				Color.White);
			
			Vector2 stick1 = Input.GetStick(1);
			_spriteBatch.Draw(_stick1, new Rectangle(center.X - (int) (BUTTON_SIZE * 3.5) + (int) (stick1.X * BUTTON_SIZE * 0.5f),
				center.Y - (int) (BUTTON_SIZE * 3.5) + (int) (-stick1.Y * BUTTON_SIZE * 0.5f), BUTTON_SIZE, BUTTON_SIZE), Color.White);

			_spriteBatch.Draw(_area_p2,
				new Rectangle(center.X + BUTTON_SIZE * 2, center.Y - BUTTON_SIZE * 4, BUTTON_SIZE * 2, BUTTON_SIZE * 2),
				Color.White);
			
			Vector2 stick2 = Input.GetStick(2);
			_spriteBatch.Draw(_stick2, new Rectangle(center.X + (int) (BUTTON_SIZE * 2.5) + (int) (stick2.X * BUTTON_SIZE * 0.5f),
				center.Y - (int) (BUTTON_SIZE * 3.5) + (int) (-stick2.Y * BUTTON_SIZE * 0.5f), BUTTON_SIZE, BUTTON_SIZE), Color.White);
			
			#endregion
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void Quit() {
			Persistence.SaveSync("global", "votes", _votes, null);
			Persistence.Flush();
			Exit();
		}
	}
}
