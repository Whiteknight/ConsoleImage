using System;
using System.Text;

namespace ConsoleImage
{
    public class ConsoleState
    {
        private readonly ConsoleColor m_backgroundColor;
        private readonly ConsoleColor m_foregroundColor;
        private readonly int m_cursorLeft;
        private readonly int m_cursorTop;
        private readonly Encoding m_outputEncoding;

        private ConsoleState(ConsoleColor backgroundColor, ConsoleColor foregroundColor, int cursorLeft, int cursorTop, Encoding outputEncoding)
        {
            m_backgroundColor = backgroundColor;
            m_foregroundColor = foregroundColor;
            m_cursorLeft = cursorLeft;
            m_cursorTop = cursorTop;
            m_outputEncoding = outputEncoding;
        }

        public static ConsoleState GetState()
        {
            return new ConsoleState(Console.BackgroundColor, Console.ForegroundColor, Console.CursorLeft, Console.CursorTop, Console.OutputEncoding);
        }

        public void ResetConsole()
        {
            Console.BackgroundColor = m_backgroundColor;
            Console.ForegroundColor = m_foregroundColor;
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
            Console.OutputEncoding = m_outputEncoding;
        }
    }

    // TODO: Implement this
    //public class RestoreOnDisposeConsoleState : IDisposable
    //{
        
    //}
}
