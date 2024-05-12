using System.Text;
using Discord.Rest;

namespace Finder.Domain.Helpers;

public static class WelcomeMessageHelper
{
    public static StringBuilder GetWelcomeMessage(RestTextChannel channel)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Welcome to the \"{channel.Name}\" channel! :tada:");
        builder.AppendLine(string.Empty);
        builder.AppendLine("This channel is created for all needed communication required during search operation. :loudspeaker:");
        builder.AppendLine("If you have any question, please feel free to reach out our administration :raised_hands:");
        return builder;
    }
}