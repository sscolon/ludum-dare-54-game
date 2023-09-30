namespace UnityEngine
{
    public static class DebugWrapper
    {

        public const string conditionalName = "UNITY_EDITOR";

        //
        // Summary:
        //     ///
        //     Reports whether the development console is visible. The development console cannot
        //     be made to appear using:
        //     ///
        public static bool developerConsoleVisible
        {
            get
            {
                return Debug.developerConsoleVisible;
            }
            set
            {
                Debug.developerConsoleVisible = value;
            }
        }
        //
        // Summary:
        //     ///
        //     In the Build Settings dialog there is a check box called "Development Build".
        //     ///
        public static bool isDebugBuild
        {
            get
            {
                return Debug.isDebugBuild;
            }
        }
        //
        // Summary:
        //     ///
        //     Get default debug logger.
        //     ///
        public static ILogger logger
        {
            get
            {
                return Debug.unityLogger;
            }
        }

        //
        // Summary:
        //     ///
        //     Assert a condition and logs an error message to the Unity console on failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.

        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition)
        {
            Debug.Assert(condition);
        }

        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition, string message)
        {
            Debug.Assert(condition, message);
        }
        //
        // Summary:
        //     ///
        //     Assert a condition and logs an error message to the Unity console on failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.

        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition, object message)
        {
            Debug.Assert(condition, message);
        }
        //
        // Summary:
        //     ///
        //     Assert a condition and logs an error message to the Unity console on failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition, Object context)
        {
            Debug.Assert(condition, context);
        }

        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition, string message, Object context)
        {
            Debug.Assert(condition, message, context);
        }
        //
        // Summary:
        //     ///
        //     Assert a condition and logs an error message to the Unity console on failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void Assert(bool condition, object message, Object context)
        {
            Debug.Assert(condition, message, context);
        }

        //
        // Summary:
        //     ///
        //     Assert a condition and logs a formatted error message to the Unity console on
        //     failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.

        [System.Diagnostics.Conditional(conditionalName)]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            Debug.AssertFormat(condition, format, args);
        }
        //
        // Summary:
        //     ///
        //     Assert a condition and logs a formatted error message to the Unity console on
        //     failure.
        //     ///
        //
        // Parameters:
        //   condition:
        //     Condition you expect to be true.
        //
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        {
            Debug.AssertFormat(condition, context, format, args);
        }
        //
        // Summary:
        //     ///
        //     Pauses the editor.
        //     ///
        [System.Diagnostics.Conditional(conditionalName)]
        public static void Break()
        {
            Debug.Break();
        }
        //
        // Summary:
        //     ///
        //     Clears errors from the developer console.
        //     ///
        [System.Diagnostics.Conditional(conditionalName)]
        public static void ClearDeveloperConsole()
        {
            Debug.ClearDeveloperConsole();
        }

        [System.Diagnostics.Conditional(conditionalName)]
        public static void DebugBreak()
        {
            Debug.DebugBreak();
        }
        //
        // Summary:
        //     ///
        //     Draws a line between specified start and end points.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            Debug.DrawLine(start, end);
        }
        //
        // Summary:
        //     ///
        //     Draws a line between specified start and end points.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Debug.DrawLine(start, end, color);
        }
        //
        // Summary:
        //     ///
        //     Draws a line between specified start and end points.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {

            Debug.DrawLine(start, end, color, duration);
        }
        //
        // Summary:
        //     ///
        //     Draws a line between specified start and end points.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
        //
        // Summary:
        //     ///
        //     Draws a line from start to start + dir in world coordinates.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawRay(Vector3 start, Vector3 dir)
        {
            Debug.DrawRay(start, dir);
        }
        //
        // Summary:
        //     ///
        //     Draws a line from start to start + dir in world coordinates.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            Debug.DrawRay(start, dir, color);
        }
        //
        // Summary:
        //     ///
        //     Draws a line from start to start + dir in world coordinates.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            Debug.DrawRay(start, dir, color, duration);
        }
        //
        // Summary:
        //     ///
        //     Draws a line from start to start + dir in world coordinates.
        //     ///
        //
        // Parameters:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [System.Diagnostics.Conditional(conditionalName)]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }
        //
        // Summary:
        //     ///
        //     Logs message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        // [System.Diagnostics.Conditional(conditionalName)]
        public static void Log(object message)
        {
            Debug.Log(message);
        }
        //
        // Summary:
        //     ///
        //     Logs message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void Log(object message, Object context)
        {
            Debug.Log(message, context);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an assertion message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogAssertion(object message)
        {
            Debug.LogAssertion(message);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an assertion message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogAssertion(object message, Object context)
        {
            Debug.LogAssertion(message, context);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted assertion message to the Unity console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            Debug.LogAssertionFormat(format, args);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted assertion message to the Unity console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            Debug.LogAssertionFormat(context, format, args);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an error message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        //    [System.Diagnostics.Conditional(conditionalName)]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an error message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogError(object message, Object context)
        {
            Debug.LogError(message, context);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted error message to the Unity console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted error message to the Unity console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            Debug.LogErrorFormat(context, format, args);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an error message to the console.
        //     ///
        //
        // Parameters:
        //   context:
        //     Object to which the message applies.
        //
        //   exception:
        //     Runtime Exception.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogException(System.Exception exception)
        {
            Debug.LogException(exception);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs an error message to the console.
        //     ///
        //
        // Parameters:
        //   context:
        //     Object to which the message applies.
        //
        //   exception:
        //     Runtime Exception.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogException(System.Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogFormat(Object context, string format, params object[] args)
        {
            Debug.LogFormat(context, format, args);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs a warning message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        //  [System.Diagnostics.Conditional(conditionalName)]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
        //
        // Summary:
        //     ///
        //     A variant of Debug.Log that logs a warning message to the console.
        //     ///
        //
        // Parameters:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogWarning(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted warning message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogWarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }
        //
        // Summary:
        //     ///
        //     Logs a formatted warning message to the Unity Console.
        //     ///
        //
        // Parameters:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [System.Diagnostics.Conditional(conditionalName)]
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            Debug.LogWarningFormat(context, format, args);
        }
    }

}