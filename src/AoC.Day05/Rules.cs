global using Rule = (int beforethan, int value);

internal record Rules(List<Rule> Value)
{
    public bool AreRespectedBy(List<int> values)
    {
        return AreRespectedBy(values, out _, out _);
    }

    public bool AreRespectedBy(List<int> values, out int notBefore, out int value)
    {
        notBefore = -1;
        value = -1;

        List<Rule> appliedRules = [];

        foreach (var rule in Value)
        {
            if (values.Contains(rule.beforethan) && values.Contains(rule.value))
            {
                appliedRules.Add(rule);
            }
        }

        foreach (var rule in appliedRules)
        {
            if (values.IndexOf(rule.beforethan) > values.IndexOf(rule.value))
            {
                notBefore = rule.beforethan;
                value = rule.value;
                return false;
            }
        }

        return true;
    }
}


internal class RulesBuilder
{
    private List<Rule> _rules = [];

    public void Add(Rule rule) => _rules.Add(rule);

    public Rules Build() => new(_rules);

}