﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP2_Xna_Template.Objects
{
    class Player
    {
        // Variables
        ContentManager Content;

        Texture2D m_Texture;
        public Rectangle m_Rectangle;

        Bullet[] m_Bullets;
        static int MAX_BULLETS = 10;
        int currentBullets = 0;
        Boolean m_CanCreateBullet = true;

        // Methods
        public Player(ContentManager content)
        {
            Content = content;
        }

        public void Initialize()
        {
            m_Texture = Content.Load<Texture2D>("player");
            m_Rectangle = new Rectangle(30, 220, 144, 72);

            m_Bullets = new Bullet[MAX_BULLETS];
        }

        public void Update()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {
                // UP or DOWN  Movement
                if (gamePadState.ThumbSticks.Left.Y > 0)
                    m_Rectangle.Y -= 3;
                if (gamePadState.ThumbSticks.Left.Y < 0)
                    m_Rectangle.Y += 3;
            }

            //------------------------------------------------------------------
            // BULLETS
            //------------------------------------------------------------------
                // Create A Bullet
                if (gamePadState.Buttons.A == ButtonState.Pressed) 
                {
                    if (m_CanCreateBullet)
                    {
                        if (currentBullets < MAX_BULLETS)
                        {
                            // Check for the first place that is empty in the bullet array (maximum of 10 bullets in the scene)
                            for (int t = 0; t < MAX_BULLETS; ++t)
                            {
                                if (m_Bullets[t] == null)
                                {
                                    // Create the bullet and go out the for loop
                                    m_Bullets[t] = new Bullet(Content);
                                    m_Bullets[t].m_Position = new Vector2(m_Rectangle.X + m_Texture.Width, m_Rectangle.Y + (m_Texture.Height / 2));
                                    m_Bullets[t].InitializePos();
                                    break;
                                }
                            }
                        }
                    }

                    // Because the Loop would otherwise make 10 Bullets at a time you have to set a boolean which enables or
                    // diables the creation of bullets
                    // When the Shoot-Button (A) is released this variable is set to true so another bullet can be created
                    m_CanCreateBullet = false;
                }

                if (gamePadState.Buttons.A == ButtonState.Released) m_CanCreateBullet = true;

                // Delete Bullets
                // Bullets get deleted when they aren't rendered on the screen (if they are to far in the X-position so they are out of the screen)
                for (int t = 0; t < MAX_BULLETS; ++t)
                {
                    if (m_Bullets[t] != null)
                    {
                        if (m_Bullets[t].GetPosition().X >= 800) m_Bullets[t] = null;
                    }
                }

            // UPDATE POSITION BULLETS
            for (int t = 0; t < MAX_BULLETS; ++t)
            {
                if (m_Bullets[t] != null)
                {
                    m_Bullets[t].Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // This is all called in the MainScene (which is the actual Game)

            // The player gets drawn
            spriteBatch.Draw(m_Texture, m_Rectangle, Color.White);

            // A loop which checks for bullets that exist and if so draws them
            for (int t = 0; t < MAX_BULLETS; ++t)
            {
                if (m_Bullets[t] != null)
                {
                    m_Bullets[t].Draw(spriteBatch);
                }
            }
        }

        // GET FUNCTIONS
        public Bullet[] GetBullets() { return m_Bullets; }

    }
}
