import cv2


# 读取图片并显示
def read_image(file_path, window):
    image = cv2.imread(file_path)
    cv2.imshow(window, image)
    cv2.waitKey(0)


# 保存图片
def save_image(file_path, image):
    cv2.imwrite(file_path, image)


# 采集视频
def read_video(video_source, window, interval=10):
    vc = cv2.VideoCapture(video_source)
    while vc.isOpened():
        ret, frame = vc.read()
        if not ret:
            print('没有读到视频帧，退出播放')
            break
        cv2.imshow(window, frame)
        key = cv2.waitKey(interval)
        if key == ord(' '):
            break
    vc.release()


# 视频录制
def save_video(video_source, video_coding, video_name, video_frame, pixel):
    vc = cv2.VideoCapture(video_source)
    fourcc = cv2.VideoWriter_fourcc(*video_coding)
    vw = cv2.VideoWriter(video_name, fourcc, video_frame, pixel)
    while vc.isOpened():
        ret, frame = vc.read()
        if not ret:
            print('没有读到视频帧，退出录制')
            break
        vw.write(frame)
        cv2.imshow('record', frame)
        key = cv2.waitKey(1000 // video_frame)  # 录制摄像头时决定帧率
        if key == ord(' '):
            break
    vc.release()
    vw.release()
