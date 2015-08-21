using System;
using System.Text;

namespace ConsoleImage
{
    public class ConsoleState
    {
        private ConsoleColor _backgroundColor;
        private ConsoleColor _foregroundColor;
        private int _cursorLeft;
        private int _cursorTop;
        private Encoding _outputEncoding;
        private bool _cursorVisible;

        private ConsoleState()
        {
        }

        public static ConsoleState GetState()
        {
            ConsoleState state = new ConsoleState();
            state._backgroundColor = Console.BackgroundColor;
            state._foregroundColor = Console.ForegroundColor;
            state._cursorLeft = Console.CursorLeft;
            state._cursorTop = Console.CursorTop;
            state._outputEncoding = Console.OutputEncoding;
            state._cursorVisible = Console.CursorVisible;
            return state;
        }

        public void ResetConsole()
        {
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _foregroundColor;
            Console.SetCursorPosition(_cursorLeft, _cursorTop);
            Console.OutputEncoding = _outputEncoding;
            Console.CursorVisible = _cursorVisible;
        }

        public IDisposable AsResetOnDispose()
        {
            return new ResetOnDisposedConsoleState(this);
        }

        private class ResetOnDisposedConsoleState : IDisposable
        {
            private readonly ConsoleState _state;
            private bool _disposed;

            public ResetOnDisposedConsoleState(ConsoleState state)
            {
                _state = state;
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                if (_disposed)
                    return;

                _state.ResetConsole();
                _disposed = true;
            }

            #endregion
        }
    }
}
