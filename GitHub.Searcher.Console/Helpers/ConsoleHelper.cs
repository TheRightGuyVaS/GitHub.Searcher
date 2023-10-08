namespace GitHub.Searcher.Console.Helpers;

public static class ConsoleHelper
{
    public static void Display(this Exception? e, int leadingTabsCount = 0)
    {
        var ex = e;
        var tabs = leadingTabsCount;

        while (ex != null)
        {
            "Message:".Display(leadingTabsCount);
            ex.Message.Display(tabs);

            if (string.IsNullOrEmpty(ex.StackTrace) == false)
            {
                "StackTrace:".Display(leadingTabsCount);
                foreach (var line in ex.StackTrace.Split(Environment.NewLine))
                {
                    line.Display(tabs);
                }
            }

            ex = ex.InnerException;
            tabs++;
        }
    }
    
    public static void Display<T>(this T? o, int leadingTabsCount = 0)
    {
        if (o == null)
        {
            return;
        }

        o.ToString().Display(leadingTabsCount);
    }

    public static void Display(this string? s, int leadingTabsCount = 0)
    {
        if (string.IsNullOrEmpty(s))
        {
            return;
        }

        System.Console.WriteLine($"{new string('\t', leadingTabsCount)}{s}");
    }

    public static void HorizontalLine()
    {
        new string('-', System.Console.WindowWidth).Display();
    }

    public static void ClearConsole() => System.Console.Clear();
    public static void NewLine() => System.Console.WriteLine();
}