namespace Finder.Domain.Models.Views;

public record LoginView(
    UserView User,
    AuthToken Token
);