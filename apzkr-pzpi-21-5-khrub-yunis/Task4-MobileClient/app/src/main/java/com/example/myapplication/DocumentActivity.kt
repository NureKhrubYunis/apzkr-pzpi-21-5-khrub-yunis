package com.example.myapplication.ui

import android.os.Bundle
import android.util.Log
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.R
import com.example.myapplication.model.Document
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import okhttp3.*
import okhttp3.logging.HttpLoggingInterceptor
import java.io.IOException
import java.security.KeyStore
import javax.net.ssl.*

class DocumentActivity : AppCompatActivity() {

    private lateinit var documentListTextView: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_document)

        documentListTextView = findViewById(R.id.documentListTextView)

        makeHttpRequest()
    }

    private fun makeHttpRequest() {
        val client = getUnsafeOkHttpClient()

        val request = Request.Builder()
            .url("https://10.0.2.2:7126/api/Mobile/documents-all")
            .build()

        Log.d("DocumentActivity", "Making request to URL: ${request.url}")

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                Log.e("DocumentActivity", "HTTP request failed: ${e.message}")
                runOnUiThread {
                    documentListTextView.text = "HTTP request failed: ${e.message}"
                }
            }

            override fun onResponse(call: Call, response: Response) {
                response.use {
                    if (!response.isSuccessful) {
                        Log.e("DocumentActivity", "HTTP request failed: ${response.message}")
                        runOnUiThread {
                            documentListTextView.text = "HTTP request failed: ${response.message}"
                        }
                        return
                    }

                    val responseBody = response.body?.string()
                    if (responseBody != null) {
                        val gson = Gson()
                        val listType = object : TypeToken<List<Document>>() {}.type
                        val documents: List<Document> = gson.fromJson(responseBody, listType)

                        val documentsText = documents.joinToString(separator = "\n") { document ->
                            """
                            ID: ${document.documentID}
                            Username: ${document.username}
                            Name: ${document.documentName}
                            Type: ${document.documentType ?: "N/A"}
                            Path: ${document.documentPath ?: "N/A"}
                            Upload Date: ${document.uploadDate}
                            """.trimIndent()
                        }

                        runOnUiThread {
                            documentListTextView.text = documentsText
                        }

                        for (document in documents) {
                            Log.i("DocumentActivity", "Document: $document")
                        }
                    } else {
                        Log.e("DocumentActivity", "Response body is null")
                        runOnUiThread {
                            documentListTextView.text = "Response body is null"
                        }
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
}
