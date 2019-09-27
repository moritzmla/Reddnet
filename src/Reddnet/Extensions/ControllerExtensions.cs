using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reddnet.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult RedirectTo<TController>(this Controller controller,
            Expression<Action<TController>> redirectExpression) where TController : Controller
        {
            if (redirectExpression.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException($"The provided expression is not a valid method call: {redirectExpression.Body}");
            }

            var methodCallExpression = redirectExpression.Body as MethodCallExpression;

            var actionName = GetActionName(methodCallExpression);
            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);
            var routeValues = ExtractRouteValues(methodCallExpression);

            return controller.RedirectToAction(actionName, controllerName, routeValues);
        }

        public static IActionResult RedirectToAsync<TController>(this Controller controller,
            Expression<Func<TController, Task>> redirectExpression) where TController : Controller
        {
            if (redirectExpression.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException($"The provided expression is not a valid method call: {redirectExpression.Body}");
            }

            var methodCallExpression = redirectExpression.Body as MethodCallExpression;

            var actionName = GetActionName(methodCallExpression);
            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);
            var routeValues = ExtractRouteValues(methodCallExpression);

            return controller.RedirectToAction(actionName, controllerName, routeValues);
        }

        private static string GetActionName(MethodCallExpression expression)
        {
            var methodName = expression.Method.Name;
            var actionName = expression.Method
                .GetCustomAttributes(typeof(ActionNameAttribute), true)
                .OfType<ActionNameAttribute>()
                .FirstOrDefault()
                ?.Name;

            return actionName ?? methodName;
        }

        private static RouteValueDictionary ExtractRouteValues(MethodCallExpression expression)
        {
            var parameters = expression.Method
                .GetParameters()
                .Select(x => x.Name)
                .ToArray();

            var values = expression.Arguments
                .Select(x =>
                {
                    if (x.NodeType == ExpressionType.Constant)
                    {
                        var constantExpression = x as ConstantExpression;
                        return constantExpression.Value;
                    }

                    var convertExpression = Expression.Convert(x, typeof(object));
                    var funcExpression = Expression.Lambda<Func<object>>(convertExpression);
                    return funcExpression.Compile().Invoke();
                })
                .ToArray();

            var routeValueDictionary = new RouteValueDictionary();

            for (int i = 0; i < parameters.Length; i++)
            {
                routeValueDictionary.Add(parameters[i], values[i]);
            }

            return routeValueDictionary;
        }
    }
}
