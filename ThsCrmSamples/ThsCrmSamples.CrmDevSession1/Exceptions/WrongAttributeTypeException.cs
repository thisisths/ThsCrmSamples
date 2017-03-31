namespace ThsCrmSamples.CrmDevSession1.Exceptions
{
    public class WrongAttributeTypeException : BusinessException
    {
        public WrongAttributeTypeException(string entity, string field, string expectedType) : base($"The field '{field}', of the entity '{entity}', does not have the epected type '{expectedType}'")
        {
            
        }
    }
}
