import time
import cv2
import pytesseract

import transfer as tf
import io
import socket

# import numpy as np

if __name__ == '__main__':
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client.connect(('127.0.0.1', 5000))
    while True:
        # print('输入指令：')
        code = input()
        if code == '1':
            image = cv2.imread('Image/Mecanim.jpg')
            cv2.imshow('rose', image[::-1, :, :])
            cv2.waitKey()
            cv2.destroyAllWindows()
        elif code == '2':
            print('输入图片名称：')
            image_name = input()
            image = cv2.imread(f'./Image/{image_name}')
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
        elif code == '3':
            config = '-l eng --oem 1 --psm 3'
            print('配置完成')
            image = cv2.imread('./Image/test1.png', cv2.IMREAD_COLOR)

            cv2.waitKey()
            cv2.destroyAllWindows()
            print('读取完成')
            text = pytesseract.image_to_string(image, config=config)
            print('转换完成')
            print(text)
        elif code == '4':
            print('信息输出')
