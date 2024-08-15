using ites.Core.Exeptions;

namespace ites.Core.Problems
{
    public static class OrderProblem
    {
        public static readonly ApiException NotFound =
            new("Заказ не найден", 404);
    }
}
