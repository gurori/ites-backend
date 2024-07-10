namespace ites.Core.Exeptions
{
    public class UserException(string message)
        : Exception(message)
    {
        public static class UserProblem
        {
            public static readonly UserException NotExistEmail = new("Пользователя с данной почтой не существует");
            public static readonly UserException WrongPassword = new("Неверный пароль");
            public static readonly UserException UserAlreadyExist = new("Данный пользователь уже существует");
            public static readonly UserException TokenProblem = new("Неправильный токен");
        }
    }
}
