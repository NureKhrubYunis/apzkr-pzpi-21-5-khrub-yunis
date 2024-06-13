package com.example.myapplication.ui

import android.content.Context
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.R
import com.example.myapplication.model.UserMessage
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.logging.HttpLoggingInterceptor
import java.io.IOException
import javax.net.ssl.*

class DirectActivity : AppCompatActivity() {

    private lateinit var directTextView: TextView
    private lateinit var receiverUsernameEditText: EditText
    private lateinit var messageEditText: EditText
    private lateinit var sendMessageButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_direct)

        directTextView = findViewById(R.id.directTextView)
        receiverUsernameEditText = findViewById(R.id.receiverUsernameEditText)
        messageEditText = findViewById(R.id.messageEditText)
        sendMessageButton = findViewById(R.id.sendMessageButton)

        val username = getUsername()
        if (username != null) {
            makeHttpRequest(username)
        } else {
            directTextView.text = "No username found. Please login first."
        }

        sendMessageButton.setOnClickListener {
            val receiverUsername = receiverUsernameEditText.text.toString()
            val message = messageEditText.text.toString()
            if (receiverUsername.isNotEmpty() && message.isNotEmpty()) {
                sendMessage(username, receiverUsername, message)
            } else {
                directTextView.text = "Please enter both receiver username and message."
            }
        }
    }

    private fun getUsername(): String? {
        val sharedPreferences = getSharedPreferences("MyAppPreferences", Context.MODE_PRIVATE)
        return sharedPreferences.getString("username", null)
    }

    private fun makeHttpRequest(username: String) {
        val client = getUnsafeOkHttpClient()

        val url = "https://10.0.2.2:7126/api/Mobile/users-with-messages/$username"
        val request = Request.Builder()
            .url(url)
            .build()

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("DirectActivity", "HTTP request failed: ${e.message}")
                runOnUiThread {
                    directTextView.text = "HTTP request failed: ${e.message}"
                }
            }

            override fun onResponse(call: Call, response: Response) {
                response.use {
                    if (!response.isSuccessful) {
                        Log.e("DirectActivity", "HTTP request failed: ${response.message}")
                        runOnUiThread {
                            directTextView.text = "HTTP request failed: ${response.message}"
                        }
                        return
                    }

                    val responseBody = response.body?.string()
                    if (responseBody != null) {
                        val gson = Gson()
                        val listType = object : TypeToken<List<UserMessage>>() {}.type
                        val userMessages: List<UserMessage> = gson.fromJson(responseBody, listType)

                        runOnUiThread {
                            directTextView.text = userMessages.joinToString(separator = "\n\n") { message ->
                                "User: ${message.user}\nLastMessage: ${message.lastMessage}\nDate: ${message.sentDate}"
                            }
                        }
                    } else {
                        Log.e("DirectActivity", "Response body is null")
                        runOnUiThread {
                            directTextView.text = "Response body is null"
                        }
                    }
                }
            }
        })
    }

    private fun sendMessage(senderUsername: String?, receiverUsername: String, message: String) {
        if (senderUsername == null) {
            runOnUiThread {
                directTextView.text = "No sender username found. Please login first."
            }
            return
        }

        val client = getUnsafeOkHttpClient()

        val url = "https://10.0.2.2:7126/api/Mobile/message"
        val requestBody = SendMessageRequest(senderUsername, receiverUsername, message)
        val json = Gson().toJson(requestBody)

        val request = Request.Builder()
            .url(url)
            .post(RequestBody.create("application/json; charset=utf-8".toMediaTypeOrNull(), json))
            .build()

        Log.d("DirectActivity", "Sending message to $url with body: $json")

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("DirectActivity", "Message send failed: ${e.message}")
                runOnUiThread {
                    directTextView.text = "Message send failed: ${e.message}"
                }
            }

            override fun onResponse(call: Call, response: Response) {
                response.use {
                    val responseBody = response.body?.string()
                    Log.d("DirectActivity", "Response: ${response.code} - $responseBody")
                    if (!response.isSuccessful) {
                        Log.e("DirectActivity", "Message send failed: ${response.message}")
                        runOnUiThread {
                            directTextView.text = "Message send failed: ${response.message}"
                        }
                        return
                    }

                    runOnUiThread {
                        directTextView.text = "Message sent successfully"
                    }
                }
            }
        })
    }

    private fun getUnsafeOkHttpClient(): OkHttpClient {
        return try {
            val trustAllCerts = arrayOf<TrustManager>(object : X509TrustManager {
                override fun checkClientTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}
                override fun checkServerTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}
                override fun getAcceptedIssuers(): Array<java.security.cert.X509Certificate> = arrayOf()
            })

            val sslContext = SSLContext.getInstance("SSL")
            sslContext.init(null, trustAllCerts, java.security.SecureRandom())

            val sslSocketFactory = sslContext.socketFactory

            val builder = OkHttpClient.Builder()
            builder.sslSocketFactory(sslSocketFactory, trustAllCerts[0] as X509TrustManager)
            builder.hostnameVerifier { _, _ -> true }

            val logging = HttpLoggingInterceptor()
            logging.setLevel(HttpLoggingInterceptor.Level.BODY)
            builder.addInterceptor(logging)

            builder.build()
        } catch (e: Exception) {
            throw RuntimeException(e)
        }
    }

    data class SendMessageRequest(val senderUsername: String, val receiverUsername: String, val text: String)
}
