import time
import cv2
import transfer as tf
import io
import socket

# import numpy as np

if __name__ == '__main__':
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client.connect(('127.0.0.1', 5000))
    while True:
        code = input('指令：')
        if code == '1':
            image = cv2.imread('Image/Mecanim.jpg')
            cv2.imshow('rose', image[::-1, :, :])
            cv2.waitKey()
            cv2.destroyAllWindows()
        elif code == '2':
            image_name = input('输入图片名称：')
            image = cv2.imread(f'Image/{image_name}')
            if image is not None:
                img_bytes = cv2.imencode('.jpg', image)[1].tobytes()
                file_name = bytes(image_name, 'utf-8')
                byte_stream = io.BytesIO(img_bytes)
                send_bytes = bytearray(4096)
                message = tf.PackageMessage(tf.TransferType.FileInfo.value, bytes(0), file_name)
                client.send(message.getbytes())
                while True:
                    length = byte_stream.readinto(send_bytes)
                    if length > 0:
                        time.sleep(0.01)
                        if length == len(send_bytes):
                            message = tf.PackageMessage(tf.TransferType.FileContent.value, send_bytes, file_name)
                        else:
                            message = tf.PackageMessage(tf.TransferType.FileContent.value, send_bytes[0:length],
                                                        file_name)
                        client.send(message.getbytes())
                    else:
                        message = tf.PackageMessage(tf.TransferType.FileEnd.value, bytes(0), file_name)
                        client.send(message.getbytes())
                        byte_stream.close()
                        break
        elif code == 'exit':
            break
