using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TabataGenerator.Helpers;
using TabataGenerator.Input;
using YamlDotNet.Serialization;

namespace TabataGenerator
{
    public class WorkoutReader
    {
        public WorkoutDescription[] Read(string input)
        {
            var workoutDescriptions = new Deserializer().Deserialize<WorkoutDescription[]>(input);

            var result = workoutDescriptions.Where(w => !w.Template).ToArray();

            EnrichFromTemplate(result, workoutDescriptions);

            return result;
        }

        private static void EnrichFromTemplate(WorkoutDescription[] result, WorkoutDescription[] workoutDescriptions)
        {
            foreach (var workoutDescription in result)
            {
                if (workoutDescription.TemplateId > 0)
                {
                    var template = workoutDescriptions.Single(w => w.Id == workoutDescription.TemplateId);

                    SetPropertyValue(workoutDescription, template, x => x.Warmup, ValidDuration);
                    SetPropertyValue(workoutDescription, template, x => x.WarmupCycles, i => i > 0);
                    SetPropertyValue(workoutDescription, template, x => x.Cycles, i => i > 1);
                    SetPropertyValue(workoutDescription, template, x => x.Work, ValidDuration);
                    SetPropertyValue(workoutDescription, template, x => x.Rest, ValidDuration);
                    SetPropertyValue(workoutDescription, template, x => x.Recovery, ValidDuration);
                    SetPropertyValue(workoutDescription, template, x => x.CoolDown, ValidDuration);
                }
            }

            static bool ValidDuration(Duration d)
            {
                return !d.IsEmpty();
            }
        }

        private static void SetPropertyValue<T, TValue>(T target, T source, Expression<Func<T, TValue>> memberLambda, Func<TValue, bool> validValue)
        {
            if (memberLambda.Body is MemberExpression memberSelectorExpression)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    var newValue = (TValue)property.GetValue(source);
                    var originalValue = (TValue)property.GetValue(target);
                    if (!validValue(originalValue) && validValue(newValue))
                    {
                        property.SetValue(target, newValue, null);
                    }
                }
            }
        }
    }
}
