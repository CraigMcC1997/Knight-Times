using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knight_Times
{
    // Class for 2D animation
    public class animation
    {
        private Texture2D image;            // Texture which holds animation sheet
        public Vector2 position;    // Position of animation
        public Rectangle rect;      // Rectangle to hold size and position
        private Rectangle frame_rect;       // Rectangle to hold position of frame to draw
        private Vector2 origin;             // Centre point
        public float rotation = 0;  // Rotation amount
        public Color colour = Color.White; // Colour
        public float size;          // Size Ratio
        public Boolean visible;     // Should object be drawn true or false
        public int framespersecond; // Frame Rate
        private int frames;                 // Number of frames of animation
        private int rows;                   // Number of rows in the sprite sheet
        private int columns;                // Number of columns in the sprite sheet
        private int frameposition;          // Current position in the animation
        private int framewidth;             // Width in pixels of each frame of animation
        private int frameheight;            // Height in pixels of each frame of animation
        private float timegone;             // Time since animation began
        private Boolean loop = false;// Should animation loop
        private int noofloops = 0;          // Number of loops to do
        private int loopsdone = 0;          // Number of loops completed
        public Boolean paused = false;  // Freeze frame animation
        private Boolean playbackwards = false;  // Sets whether animation should play forwards or backwards
        private Boolean fliphorizontal = false; // Should image be flipped horizontally

        public animation() { }


        // Constructor which initialises the animation
        public animation(ContentManager content, string spritename, int x, int y, float msize, Color mcolour, Boolean mvis, int fps, int nrows, int ncol)
        {
            image = content.Load<Texture2D>(spritename);    // Load image into texture
            position = new Vector2((float)x, (float)y);  // Set position
            rect.X = x;                                     // Set position of draw rectangle x
            rect.Y = y;                                     // Set position of draw rectangle y
            size = msize;                                   // Store size ratio
            colour = mcolour;                               // Set colour
            visible = mvis;                                 // Image visible TRUE of FALSE? 
            framespersecond = fps;                          // Store frames per second
            rows = nrows;                                   // Number of rows in the sprite sheet
            columns = ncol;                                 // Number of columns in the sprite sheet
            frames = rows * columns;                          // Store no of frames
            framewidth = (int)(image.Width / columns);      // Calculate the width of each frame of animation
            frameheight = (int)(image.Height / rows);       // Calculate the heigh of each frame of animation
            rect.Width = (int)(framewidth * size);          // Set the new width based on the size ratio    
            rect.Height = (int)(frameheight * size);	    // Set the new height based on the size ratio
            frame_rect.Width = framewidth;                  // Set the width of each frame
            frame_rect.Height = frameheight;                // Set the height of each frame
            origin.X = framewidth / 2;                      // Set X origin to half of frame width
            origin.Y = frameheight / 2;              	    // Set Y origin to half of frame heigh
        }

        // Constructor which initialises the animation
        public animation(ContentManager content, string spritename, int x, int y, float msize, Color mcolour, Boolean mvis, int fps, int nrows, int ncol, Boolean loopit, Boolean playback, Boolean drawreversed)
        {
            image = content.Load<Texture2D>(spritename);    // Load image into texture
            position = new Vector2((float)x, (float)y);  // Set position
            rect.X = x;                                     // Set position of draw rectangle x
            rect.Y = y;                                     // Set position of draw rectangle y
            size = msize;                                   // Store size ratio
            colour = mcolour;                               // Set colour
            visible = mvis;                                 // Image visible TRUE of FALSE? 
            framespersecond = fps;                          // Store frames per second
            rows = nrows;                                   // Number of rows in the sprite sheet
            columns = ncol;                                 // Number of columns in the sprite sheet
            frames = rows * columns;                          // Store no of frames
            framewidth = (int)(image.Width / columns);      // Calculate the width of each frame of animation
            frameheight = (int)(image.Height / rows);       // Calculate the heigh of each frame of animation
            rect.Width = (int)(framewidth * size);          // Set the new width based on the size ratio    
            rect.Height = (int)(frameheight * size);	    // Set the new height based on the size ratio
            frame_rect.Width = framewidth;                  // Set the width of each frame
            frame_rect.Height = frameheight;                // Set the height of each frame
            origin.X = framewidth / 2;                      // Set X origin to half of frame width
            origin.Y = frameheight / 2;              	    // Set Y origin to half of frame heigh
            loop = loopit;                                  // Should it be looped or not
            playbackwards = playback;                       // Should animation be played forwards or backwards
            fliphorizontal = drawreversed;                  // Should the animation be drawn flipped horizontally
        }

        public void start()
        {
            start(position);
        }

        public void start(Vector2 pos)
        {
            // Set position of object into the rectangle from the position Vector
            position = pos;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;

            // Start new animation
            noofloops = 0;
            visible = true;
            frameposition = 0;
            timegone = 0;
            loopsdone = 0;
            paused = false;
        }

        public void start(Vector2 pos, float rot, int repeatnumber)
        {
            // Set position of object into the rectangle from the position Vector
            position = pos;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;

            // Start new animation
            noofloops = repeatnumber;
            rotation = rot;
            visible = true;
            frameposition = 0;
            timegone = 0;
            loopsdone = 0;
            paused = false;
        }

        public void update(float gtime)
        {
            if (visible && !paused)
            {
                if (framespersecond > 0) // Error checking to avoid divide by zero
                {
                    int framepos = (int)(timegone / (1000 / framespersecond));  // Work out what frame the animation is on
                    if (framepos > frameposition) frameposition = framepos;     // Set new frame position only if it has advanced forward
                }

                // Inverse frame position number if you want to play the animation backwards
                if (playbackwards)
                    frameposition = (frames - 1) - frameposition;

                timegone += gtime;                                          // Time gone during the animation

                // Check if the animation is at the end
                if ((!playbackwards && frameposition >= frames) || (playbackwards && frameposition < 0))
                {
                    // Repeat animation if necessary
                    if (loop || loopsdone < noofloops)
                    {
                        loopsdone++;
                        if (!playbackwards)
                            frameposition = 0;
                        else
                            frameposition = frames - 1;
                        timegone = 0;
                    }
                    else
                    {
                        visible = false;   // End animation
                    }
                }
            }
        }

        // Use this method to draw the image
        public void drawme(SpriteBatch sbatch)
        {
            if (visible)
            {   // Work out the co-ordinates of the current frame and then draw that frame
                frame_rect.Y = ((int)(frameposition / columns)) * frameheight;
                frame_rect.X = (frameposition - ((int)(frameposition / columns)) * columns) * framewidth;

                if (fliphorizontal)
                    sbatch.Draw(image, rect, frame_rect, colour, rotation, origin, SpriteEffects.FlipHorizontally, 0);
                else
                    sbatch.Draw(image, rect, frame_rect, colour, rotation, origin, SpriteEffects.None, 0);
            }
        }

        // Use this method to draw the image at a specified position
        public void drawme(SpriteBatch sbatch, Vector2 newpos)
        {
            if (visible)
            {
                Rectangle newrect = rect;
                newrect.X = (int)newpos.X;
                newrect.Y = (int)newpos.Y;

                frame_rect.Y = ((int)(frameposition / columns)) * frameheight;
                frame_rect.X = (frameposition - ((int)(frameposition / columns)) * columns) * framewidth;
                if (fliphorizontal)
                    sbatch.Draw(image, newrect, frame_rect, colour, rotation, origin, SpriteEffects.FlipHorizontally, 0);
                else
                    sbatch.Draw(image, newrect, frame_rect, colour, rotation, origin, SpriteEffects.None, 0);
            }
        }

        // Use this method to draw the image at a specified position and allow image to be flipped horizontally or vertically
        public void drawme(SpriteBatch sbatch, Vector2 newpos, Boolean flipx, Boolean flipy)
        {
            if (visible)
            {
                Rectangle newrect = rect;
                newrect.X = (int)newpos.X;
                newrect.Y = (int)newpos.Y;

                frame_rect.Y = ((int)(frameposition / columns)) * frameheight;
                frame_rect.X = (frameposition - ((int)(frameposition / columns)) * columns) * framewidth;
                if (flipx)
                    sbatch.Draw(image, newrect, frame_rect, colour, rotation, origin, SpriteEffects.FlipHorizontally, 0);
                else if (flipy)
                    sbatch.Draw(image, newrect, frame_rect, colour, rotation, origin, SpriteEffects.FlipVertically, 0);
                else
                    sbatch.Draw(image, newrect, frame_rect, colour, rotation, origin, SpriteEffects.None, 0);
            }
        }
    }
}
