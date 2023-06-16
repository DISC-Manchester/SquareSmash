﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using NAudio.Wave;
using SquareSmash.utils;
using System;

namespace SquareSmash.renderer.Windows
{
    public partial class PopUpWindow : Window
    {
        private readonly Action<int, string> Callback;
        private readonly int Score;

        private void Play()
        {
            WaveOutEvent beepPlayer = new();
            beepPlayer.Init(new WaveFileReader(AssetUtil.OpenEmbeddedFile("assets.sounds.click.wav")));
            beepPlayer.Play();
        }

        public PopUpWindow()
        {
            Score = 0;
            Callback = (int n, string s) => { };
            InitializeComponent();
        }

        public PopUpWindow(int score, Action<int, string> callback)
        {
            Callback = callback;
            Score = score;
            InitializeComponent();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Play();
            if (TextInput.Text.Length >= 1 && TextInput.Text.Length <= 10)
            {
                Callback.Invoke(Score, TextInput.Text);
                Close();
                return;
            }
            TextInput.Text = "must be between 1 & 10 chars";
        }
    }
}
