using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
   public static partial class CGE
   {
      #region Ctor
      static CGE()
      {
         EventHandler onExit = (_, _) => { OnApplicationQuit?.Invoke(); };
         AppDomain.CurrentDomain.ProcessExit += onExit;
         OnApplicationStart?.Invoke();
      }
      #endregion

      #region Fields
      public static PlatformID Platform { get; private set; } = Environment.OSVersion.Platform;

      #region EngineEvents
      public static event Action OnApplicationStart;
      public static event Action OnApplicationQuit;
      #endregion
      #endregion

      #region Initialize
      public static void Initialize()
      {
         CGEUtility.DisableConsoleMode(CGEUtility.DWInputMode.ENABLE_QUICK_EDIT_MODE, out _);
         CGEUtility.OrConsoleMode(CGEUtility.DWBufferMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING, out _);
         Console.CursorVisible = false;
      }
      #endregion

      #region QOL

      #endregion
   }
}