# ImageGuide.md

The `IDToImage.csv` file allows you to map specific images to room IDs. These images appear automatically when a user approaches a room, enhancing navigation and visual branding.

### 1. File Structure

| Column | Name | Description | Type | Required |
|:-------|:-------|:------------|:-----|:---------|
| 1 | **nameId** | The unique ID of the room (e.g., E110). | String | Yes |
| 2 | **imageId** | The file name (or URI) of the image. | String | Yes |
| 3 | **width** | The width of the image. (Default: `1`) | Float | Optional* |
| 4 | **height** | The height of the image. (Default: `1`) | Float | Optional* |
| 5 | **offsetX** | Horizontal shift from center. (Default: `0`) | Float | Optional* |
| 6 | **offsetY** | Vertical shift from center. (Default: `0`) | Float | Optional* |

---

> **Note on Parameters:**&nbsp; To maintain valid parsing, parameters must be provided in pairs. If you define **width**, you must define **height**. If you define **offsets**, you must also define both **dimensions**, even if you use the default values (1.0). Make sure the dimensions and offsets are valid floats, and that the image file exists in the specified location.

---

### 2. Usage Examples

```csv
nameId, imageId, width, height, offsetX, offsetY
E110, innovationCenterIcon.png
E112, innovationCenterIcon.png, 1.5, 0.8
B115, innovationCenterIcon.png, 1.0, 0.6, 0.2, -0.45