using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScriptPortal.Vegas;

/* New object type that inherits from DockableControl */
namespace LipSyncApp
{
    public partial class LipSyncUserControl : UserControl
    {
        private TableLayoutPanel TableLayoutPanel;
        internal Vegas myVegas;
        private int index = 0;
        private List<Button> addButtons = new List<Button>();
        private NumericUpDown numericUpDown1 = new NumericUpDown();
        private Label upDownLabel = new Label();
        private int animationTrack = 0;

        public LipSyncUserControl(Vegas vegas)
        {
            this.InitializeComponent();
            this.myVegas = vegas;
        }

        private void InitializeComponent()
        {
            this.MouseWheel += OnMouseWheelScrollAndControl;
            
            this.GenerateTable(5, 2);
            this.Controls.Add(this.TableLayoutPanel);

            this.upDownLabel.Text = "Add to track (1 - 10):";
            this.upDownLabel.Size = new Size(120, 20);
            this.upDownLabel.Location = new Point(5, this.TableLayoutPanel.Height + 10);
            this.Controls.Add(this.upDownLabel);
            this.numericUpDown1.Value = 0;
            this.numericUpDown1.Maximum = 10;
            this.numericUpDown1.Minimum = 1;
            this.numericUpDown1.Size = new Size(50, 26);
            this.numericUpDown1.Location = new Point(upDownLabel.Location.X + upDownLabel.Width, upDownLabel.Location.Y);
            this.numericUpDown1.ValueChanged += (sender, args) => NumericUpDownValueChanged(numericUpDown1, args);
            this.Controls.Add(numericUpDown1);
        }

        private void GenerateTable(int columnCount, int rowCount)
        {
            this.TableLayoutPanel = new TableLayoutPanel();

            PictureBoxControl tempPicBox = new PictureBoxControl();
            int pictureBoxControlTotalHeight = tempPicBox.Height + (tempPicBox.Margin.Bottom + tempPicBox.Margin.Top);
            int pictureBoxControlTotalWidth = tempPicBox.Width + (2 * tempPicBox.Margin.Left);
            this.TableLayoutPanel.Controls.Clear();

            this.TableLayoutPanel.ColumnStyles.Clear();
            this.TableLayoutPanel.RowStyles.Clear();

            this.TableLayoutPanel.ColumnCount = columnCount;
            this.TableLayoutPanel.RowCount = rowCount;
            this.TableLayoutPanel.Size = new Size(columnCount * pictureBoxControlTotalWidth, rowCount * pictureBoxControlTotalHeight);

            for (int x = 0; x < columnCount; x++)
            {
                // First add a column
                this.TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

                for (int y = 0; y < rowCount; y++)
                {
                    // Add a row
                    if (x == 0)
                    {
                        this.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    }
                    PictureBoxControl pictureBoxControl = new PictureBoxControl();
                    pictureBoxControl.Location = new System.Drawing.Point(70, 120);
                    pictureBoxControl.PictureBox.SizeChanged += (sender, args) => OnImageChange(pictureBoxControl, args);
                    // When btn_AddToTrack is clicked, pass the PictureBoxControl to an event handler
                    pictureBoxControl.btn_AddToTrack.Click += (sender, args) => PictureBoxButton_Click(pictureBoxControl, args);
                    this.TableLayoutPanel.Controls.Add(pictureBoxControl, x, y);
                }
            }
        }

        // Adds media event to track at specified start
        TrackEvent AddMedia(Project project, string mediaPath, int trackIndex, Timecode start, Timecode length)
        {
            Media media = Media.CreateInstance(project, mediaPath);
            Track track = project.Tracks[trackIndex];
            TrackEvent currentTrackEvent = null;

            if (track.MediaType == MediaType.Video)
            {
                // If cursor isn't over an event on the track, simply add it to track
                if ((currentTrackEvent = FindTrack(project, start, trackIndex)) is null)
                    ;
                // If cursor is over an event on the track, remove the portion of that event from where
                // the cursor is to where the next event starts
                else
                {
                    // When an event is split, the left half is the original event, the right split is a new event
                    TrackEvent rightSplit = currentTrackEvent.Split(start - currentTrackEvent.Start); // offset from start
                    // rightSplit is null when you try the split right where a track begins, this if
                    // statement handles that
                    if (rightSplit is null)
                    {
                        length = currentTrackEvent.Length;
                        track.Events.Remove(currentTrackEvent);
                    }
                    else
                    {
                        length = rightSplit.Length; // set new length so new video track will fill in old tracks place
                        track.Events.Remove(rightSplit);
                    }
                }
                VideoTrack videoTrack = (VideoTrack)track;
                VideoEvent videoEvent = videoTrack.AddVideoEvent(start, length);                
                Take take = videoEvent.AddTake(media.GetVideoStreamByIndex(0));
                return videoEvent;
            }
            else
            {
                return null;
            }
        }

        // Returns the TrackEvent that the cursorPosition is found on.
        // Returns null if cursorPosition isn't over an event
        TrackEvent FindTrack(Project project, Timecode cursorPosition, int trackIndex)
        {
            for (int i = 0; i < project.Tracks[trackIndex].Events.Count; i++)
            {
                if (project.Tracks[trackIndex].Events[i].Start <= cursorPosition &&
                    project.Tracks[trackIndex].Events[i].End > cursorPosition)
                {
                    return project.Tracks[trackIndex].Events[i];
                }
            }

            return null;
        }

        private void OnMouseWheelScrollAndControl(Object Sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0) // scrolled up
                {
                    this.index++;
                    this.index %= this.addButtons.Count;
                    this.addButtons[this.index].PerformClick();
                }
                else // scrolled down
                {
                    this.index--;
                    if (this.index < 0)
                    {
                        index = addButtons.Count - 1;
                    }
                    this.addButtons[this.index].PerformClick();
                }
            }
        }

        // Event for adding image event to track
        private void PictureBoxButton_Click(Object sender, EventArgs args)
        {
            using (UndoBlock undo = new UndoBlock("Add media to track"))
            {
                try
                {
                    PictureBoxControl PictureBoxControl = sender as PictureBoxControl;
                    if (PictureBoxControl.PictureBox.ImageLocation is null)
                        ;
                    else
                    {
                        Timecode cursorPosition = this.myVegas.Transport.CursorPosition;
                        this.myVegas.SelectionStart = cursorPosition;
                        VideoEvent videoEvent = (VideoEvent)AddMedia(this.myVegas.Project, PictureBoxControl.PictureBox.ImageLocation, animationTrack, cursorPosition, Timecode.FromFrames(300));

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void NumericUpDownValueChanged(Object sender, EventArgs args)
        {
            using (UndoBlock undo = new UndoBlock("NumericUpDown value changed"))
            {
                NumericUpDown obj1 = sender as NumericUpDown;
                animationTrack = (int)obj1.Value - 1;
            }
        }


        private void OnImageChange(Object sender, EventArgs args)
        {
            PictureBoxControl pb1 = sender as PictureBoxControl;
            if (pb1.PictureBox.Image is null)
                ;
            else
            {
                this.addButtons.Add(pb1.btn_AddToTrack);
            }
        }
    }
}