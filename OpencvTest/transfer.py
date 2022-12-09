from enum import Enum


class PackageMessage:
    message_type = 0
    data_length = 0
    args_length = 0
    data = bytes(0)
    args = bytes(0)

    def __init__(self, message_type, data, args):
        self.message_type = message_type
        self.data_length = len(data)
        self.args_length = len(args)
        self.data = data
        self.args = args

    def getbytes(self):
        target_bytes = bytearray()
        target_bytes.extend(self.message_type.to_bytes(1, byteorder='little'))
        target_bytes.extend(self.data_length.to_bytes(2, 'little'))
        target_bytes.extend(self.args_length.to_bytes(2, 'little'))
        target_bytes.extend(self.data)
        target_bytes.extend(self.args)
        return bytes(target_bytes)


class TransferType(Enum):
    Text = 0
    FileInfo = 1
    FileContent = 2
    FileEnd = 3
    Picture = 4
    Shake = 5
