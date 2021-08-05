# REST TUNES API

Docker command:
```
docker run -it -p 57111:80 registry.gitlab.com/gevleeog/rest-tunes
```

Swagger:

```
http://localhost:57111/swagger/index.html
http://localhost:57111/swagger/v1/swagger.json
```

# Endpoints

### v{version:apiVersion}/playlists

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[PlaylistResult]](#collectionresult[playlistresult]) |

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| newPlaylist | body |  | No | [NewPlaylist](#newplaylist) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 201 | Success | [PlaylistResult](#playlistresult) |
| 400 | Bad Request | [ [ValidationErrorResult](#validationerrorresult) ] |
| 404 | Not Found |  |

### v{version:apiVersion}/albums/{albumId}/tracks

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| albumId | path |  | Yes | integer |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[TrackResult]](#collectionresult[trackresult]) |

### v{version:apiVersion}/albums/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [AlbumResult](#albumresult) |

### v{version:apiVersion}/playlists/{playlistId}/tracks

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| playlistId | path |  | Yes | long |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[PlaylistTrackResult]](#collectionresult[playlisttrackresult]) |
| 404 | Not Found |  |

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| playlistId | path |  | Yes | long |
| model | body |  | No | [AddTrackToPlaylist](#addtracktoplaylist) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success |  |
| 400 | Bad Request | [ [ValidationErrorResult](#validationerrorresult) ] |
| 404 | Not Found |  |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| playlistId | path |  | Yes | long |
| model | body |  | No | [DeleteTrackFromPlaylist](#deletetrackfromplaylist) |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Not Found |

### v{version:apiVersion}/playlists/{playlistId}

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| playlistId | path |  | Yes | long |
| updatedPlaylist | body |  | No | [UpdatedPlaylist](#updatedplaylist) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success |  |
| 400 | Bad Request | [ [ValidationErrorResult](#validationerrorresult) ] |
| 404 | Not Found |  |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| playlistId | path |  | Yes | long |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Not Found |

### v{version:apiVersion}/albums

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| artistId | path |  | Yes | integer |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[AlbumResult]](#collectionresult[albumresult]) |

### v{version:apiVersion}/export

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [ [ArtistExportResult](#artistexportresult) ] |

### v{version:apiVersion}/tracks/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [TrackResult](#trackresult) |
| 404 | Not Found |  |

### v{version:apiVersion}/tracks

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| albumId | path |  | Yes | integer |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[TrackResult]](#collectionresult[trackresult]) |

### v{version:apiVersion}/artists

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[ArtistResult]](#collectionresult[artistresult]) |

### v{version:apiVersion}/artists/{artistId}/albums

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| artistId | path |  | Yes | integer |
| Page | query |  | No | integer |
| Size | query |  | No | integer |
| Skip | query |  | No | integer |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [CollectionResult[AlbumResult]](#collectionresult[albumresult]) |

### v{version:apiVersion}/artists/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [ArtistResult](#artistresult) |
| 404 | Not Found |  |

### v{version:apiVersion}/playlists/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Success | [PlaylistResult](#playlistresult) |
| 404 | Not Found |  |

### Models


#### PlaylistResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| playlistId | long |  | No |
| name | string |  | No |
| _links | [ [Link](#link) ] |  | No |

#### NewPlaylist

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string |  | Yes |

#### Link

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| url | string |  | No |
| rel | string |  | No |

#### TrackResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| trackId | long |  | No |
| name | string |  | No |
| albumId | long |  | No |
| composer | string |  | No |
| duration | string |  | No |
| _links | [ [Link](#link) ] |  | No |

#### ArtistExportResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| artistId | long |  | No |
| name | string |  | No |
| albums | [ [AlbumExportResult](#albumexportresult) ] |  | No |

#### CollectionResult[PlaylistTrackResult]

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| items | [ [PlaylistTrackResult](#playlisttrackresult) ] |  | No |
| page | [Page](#page) |  | No |
| _links | [ [Link](#link) ] |  | No |

#### CollectionResult[PlaylistResult]

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| items | [ [PlaylistResult](#playlistresult) ] |  | No |
| page | [Page](#page) |  | No |
| _links | [ [Link](#link) ] |  | No |

#### AddTrackToPlaylist

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| trackId | long |  | Yes |

#### AlbumExportResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| albumId | long |  | No |
| title | string |  | No |
| tracks | [ [TrackExportResult](#trackexportresult) ] |  | No |

#### ValidationErrorResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| field | string |  | No |
| errors | [ string ] |  | No |

#### UpdatedPlaylist

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string |  | Yes |

#### PlaylistTrackResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| artist | string |  | No |
| name | string |  | No |
| id | long |  | No |
| _links | [ [Link](#link) ] |  | No |

#### Page

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| size | integer |  | No |
| totalElements | integer |  | No |
| totalPages | integer |  | No |
| number | integer |  | No |

#### CollectionResult[AlbumResult]

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| items | [ [AlbumResult](#albumresult) ] |  | No |
| page | [Page](#page) |  | No |
| _links | [ [Link](#link) ] |  | No |

#### CollectionResult[ArtistResult]

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| items | [ [ArtistResult](#artistresult) ] |  | No |
| page | [Page](#page) |  | No |
| _links | [ [Link](#link) ] |  | No |

#### CollectionResult[TrackResult]

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| items | [ [TrackResult](#trackresult) ] |  | No |
| page | [Page](#page) |  | No |
| _links | [ [Link](#link) ] |  | No |

#### ArtistResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| artistId | long |  | No |
| name | string |  | No |
| _links | [ [Link](#link) ] |  | No |

#### DeleteTrackFromPlaylist

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| trackId | long |  | Yes |

#### TrackExportResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| trackId | long |  | No |
| name | string |  | No |
| composer | string |  | No |

#### AlbumResult

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| albumId | long |  | No |
| title | string |  | No |
| _links | [ [Link](#link) ] |  | No |