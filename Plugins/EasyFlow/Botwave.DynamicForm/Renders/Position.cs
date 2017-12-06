using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Renders
{
    /// <summary>
    /// 位置对象，如果x、y相等即可认为Position相等.
    /// </summary>
    public class Position
    {
        private int x;	//水平方向
        private int y;	//垂直方向

        /// <summary>
        /// 坐标的水平方向值.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// 坐标的垂直方向值.
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        private Position() { }//强制初始化时必须赋值
        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 比较是否相等.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public override bool Equals(object otherPosition)
        {
            Position pos = otherPosition as Position;
            if (pos == null)
            {
                return false;
            }
            else
            {
                return (x == pos.X && y == pos.Y);
            }
        }

        /// <summary>
        /// 比较是否相等.
        /// </summary>
        /// <param name="otherPosition"></param>
        /// <returns></returns>
        public bool Equals(Position otherPosition)
        {
            return Equals(this, otherPosition);
        }

        /// <summary>
        /// 比较是否相等.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static bool Equals(Position pos1, Position pos2)
        {
            if (Object.ReferenceEquals(pos1, pos2))
            {
                return true;
            }

            if (Object.ReferenceEquals(pos1, null) || Object.ReferenceEquals(pos2, null))
            {
                return false;
            }
            else
            {
                return (pos1.X == pos2.X && pos1.Y == pos2.Y);
            }
        }

        /// <summary>
        /// 等于("==")操作比较.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static bool operator ==(Position pos1, Position pos2)
        {
            return Equals(pos1, pos2);
        }

        /// <summary>
        /// 不等于("!=")操作比较.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static bool operator !=(Position pos1, Position pos2)
        {
            return !Equals(pos1, pos2);
        }

        /// <summary>
        /// HashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x ^ y;
        }
    }
}
