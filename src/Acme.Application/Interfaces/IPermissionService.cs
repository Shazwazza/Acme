namespace Acme.Application.Interfaces
{
    public interface IPermissionService
    {
        void ThrowIfNoPermission(string permission);
    }
}