import transfer as tf

if __name__ == '__main__':
    my_bytes = bytes('我爱Python编程', encoding='utf-8')
    print(my_bytes)
    print(len(my_bytes))

    file_name = bytes('Mecanim.jpg', 'utf-8')
    message = tf.PackageMessage(tf.TransferType.FileInfo.value, bytes(0), file_name)
    print(message.getbytes())

    # bytestream = io.BytesIO(my_bytes)
    # send_bytes = bytearray(6)
    # while True:
    #     length = bytestream.readinto(send_bytes)
    #     if length > 0:
    #         time.sleep(0.01)
    #         if length == len(send_bytes):
    #             print(send_bytes)
    #             print(len(send_bytes))
    #         else:
    #             new_bytes = send_bytes[0:length]
    #             print(bytes(new_bytes))
    #             print(len(new_bytes))
    #     else:
    #         bytestream.close()
    #         break

    # cv2.waitKey()
