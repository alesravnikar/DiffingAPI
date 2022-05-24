# DiffingAPI

## How to build
1. Clone the repository
2. Open DiffingAPI.sln with Visual Studio 2022
3. Press F5

## How to use
Send a put request to initialize the left and right values of the diffing.

Left:
```
http://localhost:5133/v1/diff/<id>/left
```
Right:
```
http://localhost:5133/v1/diff/<id>/right
```

`<id>` can be any integer.

The put body should contain data in the following format:
```json
{
    "data": <data>
}
```
`<data>` can be any string.

To get the diffing result, make a get request to:
```
http://localhost:5133/v1/diff/<id>
```

Note that you need to use the same `<id>` for all 3 requests.
