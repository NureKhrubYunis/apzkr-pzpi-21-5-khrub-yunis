package com.example.myapplication.ui

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.example.myapplication.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        binding.openDrugActivityButton.setOnClickListener {
            val intent = Intent(this, DrugActivity::class.java)
            startActivity(intent)
        }

        binding.openDirectActivityButton.setOnClickListener {
            val intent = Intent(this, DirectActivity::class.java)
            startActivity(intent)
        }

        binding.openCommentActivityButton.setOnClickListener {
            val intent = Intent(this, CommentActivity::class.java)
            startActivity(intent)
        }

        binding.openDocumentActivityButton.setOnClickListener {
            val intent = Intent(this, DocumentActivity::class.java)
            startActivity(intent)
        }
    }
}
