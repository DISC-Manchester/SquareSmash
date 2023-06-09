﻿using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using DiscOut.Renderer;
using static Avalonia.OpenGL.GlConsts;
namespace DiscOut.Avalonia
{
    public partial class GameOpenGLControl : OpenGlControlBase
    {
        internal QuadBatchRenderer Renderer { get; private set; }
        public GameOpenGLControl()
            => InitializeComponent();

        protected override unsafe void OnOpenGlRender(GlInterface gl, int fb)
        {
            gl.ClearColor(0, 0, 0, 0);
            gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
            gl.Viewport(0, 0, (int)DiscWindow.Instance.Width - 25, (int)DiscWindow.Instance.Height);
            DiscWindow.Instance.Paddle.OnRendering(Renderer);
            DiscWindow.Instance.Level.OnRendering(Renderer);
        }

        protected override void OnOpenGlInit(GlInterface gl, int fb)
            => Renderer = new QuadBatchRenderer(gl);

        protected override void OnOpenGlDeinit(GlInterface gl, int fb)
            => Renderer.Dispose();
    }
}