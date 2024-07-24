using ites.Core.Exeptions;

namespace ites.Core.Problems
{
    public static class UserProblem
    {
        public static readonly ApiException NotExistEmail = 
            new("Пользователя с данной почтой не существует", 404);

        public static readonly ApiException NotExistFile =
            new("Файла не существует", 404);

        public static readonly ApiException WrongPassword = 
            new("Неверный пароль", 409);

        public static readonly ApiException UserAlreadyExist = 
            new("Данный пользователь уже существует", 409);

        public static readonly ApiException TokenProblem = 
            new("Проблемы с токеном", 401);

        public static readonly ApiException NotFound =
            new("Пользователь не найден", 404);
    }
}
