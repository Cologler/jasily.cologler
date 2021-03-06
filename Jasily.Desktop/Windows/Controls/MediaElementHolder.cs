﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Jasily.Windows.Controls
{
    public class MediaElementHolder : INotifyPropertyChanged
    {
        private readonly MediaElement mediaElement;
        private MediaState currentStatus;
        private bool isPlaying;
        public event EventHandler<MediaState> StatusChanged;

        public MediaElementHolder(MediaElement mediaElement)
        {
            this.mediaElement = mediaElement;
            this.mediaElement.MediaOpened += this.MediaElement_MediaOpened;
            this.mediaElement.MediaEnded += this.MediaElement_MediaEnded;
            this.mediaElement.MediaFailed += this.MediaElement_MediaFailed;
        }

        private void MediaElement_MediaFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
            => this.CurrentStatus = MediaState.Close;

        private void MediaElement_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
            => this.CurrentStatus = this.mediaElement.UnloadedBehavior;

        private void MediaElement_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
            => this.CurrentStatus = MediaState.Play;

        public MediaState CurrentStatus
        {
            get { return this.currentStatus; }
            private set
            {
                if (this.currentStatus == value) return;
                this.IsPlaying = value == MediaState.Play;
                this.currentStatus = value;
                this.StatusChanged?.Invoke(this, value);
            }
        }

        public void Play()
        {
            this.CurrentStatus = MediaState.Play;
            this.mediaElement.Play();
        }

        public void Pause()
        {
            this.CurrentStatus = MediaState.Pause;
            this.mediaElement.Pause();
        }

        public void Stop()
        {
            this.CurrentStatus = MediaState.Stop;
            this.mediaElement.Stop();
        }

        public void Close()
        {
            this.CurrentStatus = MediaState.Close;
            this.mediaElement.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool IsPlaying
        {
            get { return this.isPlaying; }
            set
            {
                if (this.isPlaying == value) return;
                this.isPlaying = value;
                this.OnPropertyChanged();
            }
        }
    }
}