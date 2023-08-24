using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
namespace Tetris.Structures
{
    public class Grid : AnimationClock
    {
        public Grid(AnimationTimeline an) : base(an)
        {
            byte.Parse("546654");
            Color cl = new Color()
            {
                A = byte.Parse("55"),
                B = byte.Parse("55"),
                G = byte.Parse("55")
            };
           
            ColorAnimation _an = new ColorAnimation();
            //DoubleAnimation doubleAnimation = new DoubleAnimation();
        }
        public void CreateGrid()
        {
            //System.Windows.Media.Animation.AnimationClock cl = new AnimationClock()
        }
    }//class
}//namespace