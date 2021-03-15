using System;
using GameOverlay.Drawing;
using PapeOverlay;
using PapeOverlay.Core;
using PapeOverlay.Core.Layers;

namespace CustomOverlay
{
    public class CustomOverlay : OverlayApplication
    {
        private GIFImageLayer duckGif;
        private TextLayer textDuck;


        public CustomOverlay(string path) : base(path)
        {

        }

        public override AppConfig DefaultConfig()
        {
            // Default config for your overlay
            return new AppConfig()
            {
                AuthorName = "MyAuthorName",
                DescriptionOverlay = "My Description!",
                Version = "1.0.0",
                DestinationWindow = "Notepad"
            };
        }

        protected override void InitializeComponents(WindowOverlay windowOverlay)
        {
            // Require anything you need here!
            windowOverlay.SetBrush("red", Color.Red);
            windowOverlay.SetBrush("white", new Color(255, 255, 255));

            // Any assets goes is relative to the executable in the path ./Assets/xxxxxxx
            windowOverlay.SetImage("duckAnimated", AssetsPath + "/duckGif.gif");
            windowOverlay.SetFont("arial", "Arial", 16);
        }

        protected override void OnCreateLayers()
        {
            // Create every layer you need in here
            Point windowSize = WindowOverlay.GetWindowSize(); 
            Layer rootLayer = new Layer()
            {
                Name = "Root",
            };
            rootLayer.Transform.Position = windowSize.GetCenter();

            duckGif = new GIFImageLayer()
            {
                Name = "Ducky Duck",
            };
            duckGif.SetImageName("duckAnimated");
            duckGif.Transform.Parent = rootLayer.Transform;
            duckGif.Transform.RelativePosition = new Point(-rootLayer.Transform.Position.X / 2, 0);

            textDuck = new TextLayer()
            {
                Name = "Duck Text",
                Text = "Ducks Are COOOOL!",
                FontSize = 24
            };
            textDuck.SetBrushColorName("red");
            textDuck.SetFontName("arial");
            textDuck.Transform.Parent = rootLayer.Transform;
            textDuck.Transform.RelativePosition = new Point(rootLayer.Transform.Position.X / 2, 0);


            WindowOverlay.RootLayer = rootLayer;
        }

        protected override void OnUpdateOverlay(float deltaTime, long frameCount, float frameTime)
        {
            
        }
    }
}
