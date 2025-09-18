# Write your code here :-)
a = int(input("Nhập vào một số:"))
b = a%5
c = a%3
if b==0 and c ==0:
    print("FIZZ BUZZ")
elif b ==0:
    print("FIZZ")
elif c == 0:
    print("BUZZ")
