
namespace IMS.Application.Abstractions.Caching
{
    // Service for managing cache versions based on prefixes

    // This is useful for invalidating groups of cache entries
    // by incrementing a version number associated with a prefix
    // For example, if cache entries are stored with keys like "user:123:v1" 
    // incrementing the version for the "user" prefix will invalidate 
    // all user-related cache entries 
    // without needing to track each individual key
    // This approach improves cache management efficiency and scalability 
    // especially in applications with many dynamic cache entries
    // Example usage: 
    // - GetVersionAsync("user") returns 1 
    // - Cache entries for users are stored as "user:{id}:v1"
    // - IncrementAsync("user") increments the version to 2
    // - Subsequent cache lookups for users will use "user:{id}: v2" 
    //  effectively invalidating the previous entries
    // This service can be integrated with caching behaviors 
    //  to automate cache versioning and invalidation 
    // throughout the application 
    public interface ICacheVersionService
    {
        // Gets the current version number for the specified prefix
        // If no version exists, it returns 0 
        int GetVersion(string prefix);

        // Increments the version number for the specified prefix
        // This invalidates all cache entries associated with that prefix
        void Increment(string prefix);
    }
}
