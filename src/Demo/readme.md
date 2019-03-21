This demo illustrates a couple things:
- Isolating the domain from the repo implementation without causing the domain to become anemic (too many DTOs)
- Using a proper DDD aggregate pattern: Per context, there is an aggregate with a root through which all operations must be controlled.
- Using strong objects in the domain to represent simple things like IDs which reduces developer error (accidentally sending or mapping the wrong id)
-
