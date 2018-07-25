# MNIST Extractor
A C# console application to extract images from MNIST dataset and save it to 10 directories (0 to 9) each crossponding to its class.
# How to use
- Download [MNIST Dataset](http://yann.lecun.com/exdb/mnist/ "MNIST Dataset")
- Put and Extract it in executable directory
- Run Extractor
- Enter Images and Labels File (e.g `t10k-images.idx3-ubyte` and `t10k-labels.idx1-ubyte` for training and `train-images.idx3-ubyte` and `train-labels.idx1-ubyte` for testing)
- Enter output directory
# Output
It will generate a directory and create 10 directories name ranging from 0 to 9. It will extract each image from MNIST dataset and will place it into the directory it is crossponds to.