import bpy
import os
import json
from bpy_extras.io_utils import ImportHelper
from bpy.props import StringProperty
from bpy.types import Operator

# https://sinestesia.co/blog/tutorials/using-blenders-filebrowser-with-python/
class KRT_RestoreShapeKeysOpenFilebrowser(Operator, ImportHelper):

    bl_idname = "restore_shape_keys.open_filebrowser"
    bl_label = "Open"

    filter_glob: StringProperty(
        default='*.json',
        options={'HIDDEN'}
    )

    def execute(self, context):
        """Do something with the selected file(s)."""
        filename, extension = os.path.splitext(self.filepath)
        print('Selected file:', self.filepath)
        
        # Load data
        with open(self.filepath, mode='rt', encoding='utf-8') as file:
            keyData: dict = json.load(file)

        # Update shape key name
        for obj in bpy.data.objects:
            if obj.name in keyData:
                keys = keyData[obj.name]
                mesh = obj.to_mesh()
                if hasattr(mesh.shape_keys, 'key_blocks'):
                    for keyblock in mesh.shape_keys.key_blocks:
                        if keyblock.name in keys:
                            print(f'{obj.name}: {keyblock.name} -> {keys[keyblock.name]}')
                            keyblock.name = keys[keyblock.name]

        return {'FINISHED'}

def register():
    bpy.utils.register_class(KRT_RestoreShapeKeysOpenFilebrowser)
    
def unregister():
    bpy.utils.unregister_class(KRT_RestoreShapeKeysOpenFilebrowser)
    
if __name__ == "__main__":
    register()
    
    bpy.ops.restore_shape_keys.open_filebrowser('INVOKE_DEFAULT')
