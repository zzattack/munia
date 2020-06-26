from django import forms
from snowpenguin.django.recaptcha3.fields import ReCaptchaField

class ContactForm(forms.Form):
    name = forms.CharField(label='Your name', initial='Anonymous', max_length=255)
    email = forms.EmailField(label='Email', max_length=255)
    message = forms.CharField(label='Message', widget=forms.Textarea(attrs={'placeholder': 'I have a question regarding the MUNIA product'}))
    captcha = ReCaptchaField()


